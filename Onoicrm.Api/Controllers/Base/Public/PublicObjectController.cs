using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;

namespace Onoicrm.Api.Controllers.Base.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
public abstract class PublicObjectController<TEntity> : ReadOnlyObjectController<TEntity> where TEntity : Entity, new()
{
    
    protected PublicObjectController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public virtual async Task<IActionResult> Add(TEntity model) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () => await Context.Set<TEntity>().AddAsync(model));
        var result = await GetModel(m => m.Id==model.Id);
        return result;
    });
    
    [HttpPut("{id:long}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public virtual async Task<IActionResult> Update(TEntity model) => await ExecuteRequest(async () => await ExecuteDbCommand(() => Context.Entry(model).State = EntityState.Modified));
    
    [HttpDelete("{id:long}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public virtual async Task<IActionResult> Delete(long id)=> await ExecuteRequest(async () => await ExecuteDbCommand(async () =>
    {
        var model = await GetModelForDelete(m=>m.Id==id);
        await BeforeDelete(model);
        Context.Set<TEntity>().Remove(model);
    }));
    
    [HttpPatch("{id:long}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public async Task<IActionResult> UpdateField(long id,[FromBody] List<JsonDocument> updateObjects) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () =>
        {
            var model = await Context.Set<TEntity>().FindAsync(id);
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
        });
        
        var result = await GetModel(e => e.Id == id);
        return result;
    });

    [HttpPost("range")]
    public virtual async Task<IActionResult> AddRange([FromBody] List<TEntity> items)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(async () =>
            {
                await Context.Set<TEntity>().AddRangeAsync(items);
            });

            var itemIds = items.Select(i => i.Id);
            
            var result = await Context.Set<TEntity>()
                .Where(e => itemIds.Contains(e.Id))
                .ToListAsync();

            return result;
        });
    }

    
    [HttpDelete("range")]
    public virtual async Task<IActionResult> DeleteRange([FromBody] ICollection<TEntity> items) => 
        await ExecuteRequest(async () => 
            await ExecuteDbCommand(()=> Context.Set<TEntity>().RemoveRange(items))
        );

    [HttpPatch("priority")]
    public virtual async Task<IActionResult> UpdatePriority(List<UpdatePriorityModel> priorityModels)
    {
        return await ExecuteRequest(async () =>
        {
            var ids = priorityModels.Select(i => i.Id).ToList();
            var items =  await Context.Set<TEntity>()
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
            
            foreach (var priorityModel in priorityModels)
            {
                var model = items.First(i => i.Id == priorityModel.Id);
                model.Priority = priorityModel.Priority;
                Context.Set<TEntity>().Update(model);
            }

            await Context.SaveChangesAsync();
        });
    }
    
    protected virtual async Task<TEntity> GetModelForDelete(Expression<Func<TEntity, bool>> expression) => await GetModel(expression);
    protected virtual Task BeforeDelete(TEntity model) => Task.CompletedTask;
}