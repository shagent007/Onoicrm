using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Exceptions;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Services;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Admin;

[Route("api/v1/admin/[controller]")]
[ApiController]
[Authorize(Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class UserProfileController :  ReadOnlyObjectController<UserProfile>
{
    private readonly IUserService _userService;
    public UserProfileController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager, IUserService userService) : base(configuration, dataContext, userManager)
    {
        _userService = userService;
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
        catch (UserRegistrationException e)
        {
            return BadRequest(e);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update(UserProfile model) => await ExecuteRequest(async () => await ExecuteDbCommand(() => Context.Entry(model).State = EntityState.Modified));
    
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
        return await ExecuteDbCommand(async () =>
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
                if (prop == null || !prop.CanWrite) continue;
                var t = prop.PropertyType;
                var fieldValue = field.RootElement.GetProperty("fieldValue");
                var safeValue = fieldValue.Deserialize(t,
                    new JsonSerializerOptions()
                        { NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString });
                prop.SetValue(model, safeValue, null);
            }

            return model;
        });
    });

    protected override IQueryable<UserProfile> FilterPredicate(Filter filter, IQueryable<UserProfile> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "id":
                return result.Where(e => e.Id == filter.Value.GetInt64());
            case "fullName":
                return result.Where(e =>
                    EF.Functions.Like(e.FirstName.ToUpper(), $"%{filter.Value.GetString()}%".ToUpper()));
           
            case "roleNames":
            {
                var roleNames = filter.Value.GetString()!.Split($",").ToList();
                return result.Where(up => 
                    Context.Roles.Any(r => 
                        roleNames.Contains(r.Name ?? "")
                        && 
                        Context.UserRoles.Any(ur => 
                            ur.RoleId == r.Id && ur.UserId == up.UserId
                        )
                    )
                ); 
            }
            case "excludeRoleNames":
            {
                var roleNames = filter.Value.GetString()!.Split($",").ToList();
                return result.Where(up => 
                    Context.Roles.Any(r => 
                        roleNames.Contains(r.Name)
                        && 
                        !Context.UserRoles.Any(ur => 
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
                return result;
        }
        
    }

    protected virtual async Task<UserProfile> GetModelForDelete(Expression<Func<UserProfile, bool>> expression)
    {
        var model = await Context.Set<UserProfile>()
            .Include(up => up.User)
            .SingleAsync(expression);

        var userRoles = await Context.UserRoles
            .Where(ur => ur.UserId == model.UserId)
            .ToListAsync();

        var symptoms = await Context.Set<PatientSymptom>()
            .Where(ups => ups.PatientId == model.Id)
            .ToListAsync();

     

        Context.Set<PatientSymptom>().RemoveRange(symptoms);
        Context.UserRoles.RemoveRange(userRoles);
        await Context.SaveChangesAsync();
        
        return model;
    }

    protected virtual Task BeforeDelete(UserProfile model) => Task.CompletedTask;
}