using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Admin;

[Route("api/v1/admin/[controller]")]
[ApiController]
[Authorize(Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class RoleController :  BaseController
{
    
    public RoleController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager) : base(configuration, context, userManager)
    {
    }
    
    [HttpPost]
    public async Task<IActionResult> Add(IdentityRole model) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () => await Context.Roles.AddAsync(model));
        var result = await GetModel(m => m.Id==model.Id);
        return result;
    });
    
    protected virtual IQueryable<IdentityRole> FilterPredicate(Filter filter, IQueryable<IdentityRole> entities)
    {
        return entities;
    }

    protected virtual async Task<IdentityRole> GetModel(Expression<Func<IdentityRole, bool>> expression)
    {
        var model = await Context.Roles.SingleAsync(expression);
        return model;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetList(int? pageIndex, int? pageSize, string? orderFieldName, string? orderFieldDirection, string? filter = null) => await ExecuteRequest(async () =>
    {
        var config = new ListConfig(pageIndex, pageSize, orderFieldName, orderFieldDirection, filter);
        var query = Context.Roles
            .Filter(config.Filter, FilterPredicate)
            .Sort(config.OrderFieldName, config.OrderFieldDirection);

        var items = await query.Paginate(config.PageIndex, config.PageSize).ToListAsync();
        var total = query.Count();
        var result = new ListResponse(items, total);
        return result;
    });

    [HttpGet("{id}")]
    public virtual async Task<IActionResult> GetById(string id) =>  await ExecuteRequest(async () =>
    {
        var model = await GetModel(m => m.Id == id);
        return model;
    });

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(IdentityRole model) => await ExecuteRequest(async () => await ExecuteDbCommand(() => Context.Entry(model).State = EntityState.Modified));
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)=> await ExecuteRequest(async () => await ExecuteDbCommand(async () =>
    {
        var model = await Context.Roles.FirstOrDefaultAsync(m=>m.Id==id);
        if (model == null) throw new Exception("Роль не надено");
        Context.Roles.Remove(model);
    }));

}