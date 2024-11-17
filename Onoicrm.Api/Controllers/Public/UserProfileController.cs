using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Services;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class UserProfileController :  ReadOnlyObjectController<UserProfile>
{
    private readonly IUserService _userService;
    private readonly WappiService _wappiService;
    private readonly UserManager<IdentityUser> _userManager;
    public UserProfileController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager, IUserService userService, WappiService wappiService) : base(configuration, dataContext, userManager)
    {
        _userManager = userManager;
        _userService = userService;
        _wappiService = wappiService;
    }

    protected override async Task<UserProfile> GetModel(Expression<Func<UserProfile, bool>> expression)
    {
        var model = await Context.Set<UserProfile>()
            .Include(up => up.User)
            .SingleAsync(expression);
        
        model.Roles = await UserManager.GetRolesAsync(model.User!);
        
        return model;
    }

    [HttpGet("with-roles")]
    public async Task<IActionResult> GetWithRoles(int? pageIndex, int? pageSize, string? orderFieldName, string? orderFieldDirection, string? filter = null) => await ExecuteRequest(async () =>
    {
        var config = new ListConfig(pageIndex, pageSize, orderFieldName, orderFieldDirection, filter);
        var query = Context.Set<UserProfile>()
            .Include(up => up.User)
            .Filter(config.Filter, FilterPredicate)
            .Sort(config.OrderFieldName, config.OrderFieldDirection);

        var items = await query.Paginate(config.PageIndex, config.PageSize).ToListAsync();
        
        foreach (var userProfile in items)
        {
            userProfile.Roles = await UserManager.GetRolesAsync(userProfile.User!);
        }
        
        var total = query.Count();
        var result = new ListResponse(items, total);
        return result;
    });


    [HttpGet("{id:long}/doctor/invoice/{clinicId:long}/service-percent")]
    public async Task<IActionResult> GetDoctorInvoices(long id, long clinicId)
    {
        return await ExecuteRequest(async () =>
        {
            var doctor = await Context.Set<UserProfile>().FindAsync(id);
            if (doctor == null) throw new NullReferenceException();
            var invoices = await Context.Set<ImplementedService>()
                .Select(s => new
                {
                    s.BookingId,
                    s.DoctorId,
                    s.Booking.ClinicId,
                    s.Price,
                    s.Count,
                    s.Salary,
                    s.Booking.LastUpdate,
                    s.Booking.CreateDate,
                    PatientFullName = s.Booking.Patient.FullName
                })
                .Where(s => s.DoctorId == id && s.ClinicId == clinicId)
                .GroupBy(s => s.BookingId)
                .Select(g => new
                {
                    BookingId = g.Key,
                    Sum = g.Sum(x => x.Price * x.Count),
                    Percent = g.Sum(x => x.Salary / 100.0 * (x.Price * x.Count)),
                    Date = g.Select(x => x.LastUpdate).FirstOrDefault(),
                    Patient = g.Select(x => x.PatientFullName).FirstOrDefault()
                })
                .ToListAsync();
            
            return invoices;
        });
    }
    
   
    
    [HttpPost("mailing/category")]
    public async Task<IActionResult> MailingByCategory([FromBody] CategoryMailingModel model)
    {
        try
        {
            var today = DateTimeOffset.Now.ToUniversalTime();
            var bookingTeeth = await Context.Set<BookingTooth>()
                .Where(bt =>
                    bt.ToothStateId != null
                    &&
                    Context.Set<Booking>().Any(b => 
                        bt.BookingId == b.Id
                        &&
                        b.ClinicId == model.ClinicId 
                        && 
                        b.DateTimeStart <= today
                    )
                )
                .ToListAsync();


            var bookingToothIds = bookingTeeth
                .Where(bt => model.Categories.Contains(bt.ToothStateId))
                .GroupBy(bt => bt.ToothId)
                .Select((g, k) => g.MaxBy(bt => bt.CreateDate).BookingId)
                .ToList();

            var clinic = await Context.Set<Clinic>().FindAsync(model.ClinicId);
            if (clinic == null) throw new Exception("Клиника не найдено");
            var userProfiles = await Context.Set<UserProfile>()
                .Where(up => Context
                    .Set<Booking>()
                    .Any(b => bookingToothIds.Contains(b.Id) && b.PatientId == up.Id)
                )
                .ToListAsync();

            foreach (var userProfile in userProfiles)
            {
                await _wappiService.SendMessageAsync(new WhatsAppMessage
                {
                    Text = model.Text.ReplaceTemplate(userProfile),
                    To=userProfile.WhatsAppNumber.ClearPhoneNumber(),
                    ProfileId = clinic.WappiProfileId,
                    Token = clinic.WappiToken
                });
            }
          
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
       

    }


    [HttpGet("{id:long}/tooth-states")]
    public async Task<IActionResult> GetToothStates(long id, long clinicId)
    {
        return await ExecuteRequest(async () =>
        {
            var bookingTeeth = await Context.Set<BookingTooth>()
                .Include(bt => bt.Tooth)
                .Include(bt => bt.ToothState)
                .Where(bt =>
                    bt.ToothStateId != null
                    &&
                    Context.Set<Booking>().Any(b => 
                        b.PatientId == id
                        &&
                        bt.BookingId == b.Id
                        &&
                        b.ClinicId == clinicId 
                    )
                )
                .ToListAsync();


            var toothStateModels = bookingTeeth
                .GroupBy(bt => bt.Tooth)
                .Select((g, k) => new
                {
                    position = g.Key.Position,
                    color = g.MaxBy(bt => bt.CreateDate).ToothState?.Color
                })
                .ToList();

            return toothStateModels;
        });
    }
    
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] RegisterModel model)
    {
        try
        {
            if (!ModelState.IsValid) throw new ArgumentException("Передан не правельный модель");
            await _userService.RegisterUserAsync(model);
            var defined = await UserManager.FindByEmailAsync(model.Email);
            var userProfile = await GetUserProfile(defined);
            return Ok(userProfile);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(UserProfile model)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(() =>
            {
                Context.Set<UserProfile>().Update(model);
            });
        });
    }

    [HttpGet("check/{userName}")]
    public async Task<IActionResult> CheckUserName(string userName)
    {
        return await ExecuteRequest(async () =>
        {
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null) throw new Exception("Имя пользователя уже занято");
            return Ok();
        });
    }
    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)=> await ExecuteRequest(async () => await ExecuteDbCommand(async () =>
    {
        var model = await GetModelForDelete(m=>m.Id==id);
        await BeforeDelete(model);
        Context.Set<UserProfile>().Remove(model);
    }));
    
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateField(long id,[FromBody] List<JsonDocument> updateObjects) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () =>
        {
            var model = await Context.Set<UserProfile>().FindAsync(id);
            if (model == null) throw new ArgumentException($"Обьект с id={id} не найдено");
            foreach (var field in updateObjects)
            {
                var prop = model.GetType()
                    .GetProperty(
                        (field.RootElement.GetProperty("fieldName").GetString() ??
                         throw new InvalidOperationException()).ToPascalCase(),
                        BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) continue;
                var t = prop.PropertyType;
                var fieldValue = field.RootElement.GetProperty("fieldValue");
                var safeValue = fieldValue.Deserialize(t,
                    new JsonSerializerOptions()
                        { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString });

                var valueType = safeValue.GetType();
                switch (valueType.Name)
                {
                 
                    default:
                    {
                        prop.SetValue(model, safeValue, null);
                        break;
                    }
                }
                
            }
        });

        var result = await GetModel(u => u.Id == id);
        return result;
    });
    
    protected override IQueryable<UserProfile> FilterPredicate(Filter filter, IQueryable<UserProfile> entities)
    {
        switch (filter.Name)
        {
            case "id":
                return entities.Where(e => e.Id == filter.Value.GetInt64());
            case "searchText":
                return entities.Where(e =>
                    EF.Functions.Like(e.WhatsAppNumber.ToUpper(), $"%{filter.Value.GetString()}%".ToUpper())
                    ||
                    EF.Functions.Like(e.FullName.ToUpper(), $"%{filter.Value.GetString()}%".ToUpper())
                );
      
            case "excludeRoleNames":
            {
                var roleNames = filter.Value.GetString()!.Split($",").ToList();
                return entities.Where(up => 
                    Context.Roles.Any(r => 
                        roleNames.Contains(r.Name)
                        && 
                        !Context.UserRoles.Any(ur => 
                            ur.RoleId == r.Id && ur.UserId == up.UserId
                        )
                    )
                ); 
            }
            case "roleNames":
            {
                var roleNames = filter.Value.GetString()!.Split($",").ToList();
                return entities.Where(up => 
                    Context.Roles.Any(r => 
                        roleNames.Contains(r.Name ?? "")
                        && 
                        Context.UserRoles.Any(ur => 
                            ur.RoleId == r.Id && ur.UserId == up.UserId
                        )
                    )
                ); 
            }
            case "clinicId":
            {
                return entities.Where(up =>  up.ClinicId == filter.Value.GetInt32()); 
            }   
      
            default:
                return entities;
        }
    }
    
    protected virtual async Task<UserProfile> GetModelForDelete(Expression<Func<UserProfile, bool>> expression) => await GetModel(expression);
    protected virtual Task BeforeDelete(UserProfile model) => Task.CompletedTask;
}