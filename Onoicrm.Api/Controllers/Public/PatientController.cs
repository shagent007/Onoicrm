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
public class PatientController :  ReadOnlyObjectController<Patient>
{
    public PatientController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext, userManager)
    {
    }

    protected override async Task<Patient> GetModel(Expression<Func<Patient, bool>> expression)
    {
        var model = await Context.Set<Patient>().SingleAsync(expression);
        return model;
    }
    
    [HttpGet("{id:long}/invoice/{clinicId:long}/groupId/{groupId:long}")]
    public async Task<IActionResult> GetPatientInvoices(long id, long clinicId, long groupId)
    {
        return await ExecuteRequest(async () =>
        {

            var bookings = await Context.Set<Booking>()
                .Include(b => b.Doctor)
                .Include(b => b.BookingTeeth)
                .Where(b => b.ClinicId == clinicId && b.PatientId == id)
                .ToListAsync();
            

            var bookingIds = bookings.Select(b => b.Id).ToList();
            
            var invoices = await Context.Set<ImplementedService>()
                .Where(s => bookingIds.Contains(s.BookingId))
                .Include(s => s.Service)
                .GroupBy(s => s.BookingId)
                .Select(s => new
                {
                    Id = s.Key,
                    Sum = s.Select(service => service.Service.Price * service.Count).Sum(),
                })
                .ToListAsync();

            var implementedServices = invoices
                .Select(s =>
                {
                    var booking = bookings.First(b => b.Id == s.Id);

                    return new
                    {
                        s.Id,
                        s.Sum,
                        Date=booking.DateTimeStart,
                        booking.Discount,
                        booking.DiscountType,
                        Total = booking.GetTotal(s.Sum),
                        booking.Doctor,
                        booking.BookingTeeth
                    };
                })
                .ToList();
            
            
            return implementedServices;
        });
    }
    

    [HttpGet("{id:long}/invoice/{clinicId:long}")]
    public async Task<IActionResult> GetPatientInvoices(long id, long clinicId)
    {
        return await ExecuteRequest(async () =>
        {

            var bookings = await Context.Set<Booking>()
                .Include(b => b.Doctor)
                .Include(b => b.BookingTeeth)
                .Where(b => b.ClinicId == clinicId && b.PatientId == id)
                .ToListAsync();

            var bookingIds = bookings.Select(b => b.Id).ToList();
            
            var invoices = await Context.Set<ImplementedService>()
                .Where(s => bookingIds.Contains(s.BookingId))
                .Include(s => s.Service)
                .GroupBy(s => s.BookingId)
                .Select(s => new
                {
                    Id = s.Key,
                    Sum = s.Select(service => service.Service.Price * service.Count).Sum(),
                })
                .ToListAsync();

            var implementedServices = invoices
                .Select(s =>
                {
                    var booking = bookings.First(b => b.Id == s.Id);

                    return new
                    {
                        s.Id,
                        s.Sum,
                        Date=booking.DateTimeStart,
                        booking.Discount,
                        booking.DiscountType,
                        Total = booking.GetTotal(s.Sum),
                        booking.Doctor,
                        booking.BookingTeeth
                    };
                })
                .ToList();
            
            
            return implementedServices;
        });
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
    public async Task<IActionResult> Add([FromBody] Patient model)
    {
        try
        {
            if (!ModelState.IsValid) throw new ArgumentException("Передан не правельный модель");
            Context.Set<Patient>().Add(model);
            await Context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(Patient model)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(() =>
            {
                Context.Set<Patient>().Update(model);
            });
        });
    }

    
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)=> await ExecuteRequest(async () => await ExecuteDbCommand(async () =>
    {
        var model = await GetModelForDelete(m=>m.Id==id);
        await BeforeDelete(model);
        Context.Set<Patient>().Remove(model);
    }));
    
    [HttpPatch("{id:long}")]
    public async Task<IActionResult> UpdateField(long id,[FromBody] List<JsonDocument> updateObjects) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () =>
        {
            var model = await Context.Set<Patient>().FindAsync(id);
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
    
    protected override IQueryable<Patient> FilterPredicate(Filter filter, IQueryable<Patient> entities)
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
            
            case "clinicId":
            {
                return entities.Where(up =>  up.ClinicId == filter.Value.GetInt32()); 
            }   
      
            default:
                return entities;
        }
    }
    
    protected virtual async Task<Patient> GetModelForDelete(Expression<Func<Patient, bool>> expression) => await GetModel(expression);
    protected virtual Task BeforeDelete(Patient model) => Task.CompletedTask;
}