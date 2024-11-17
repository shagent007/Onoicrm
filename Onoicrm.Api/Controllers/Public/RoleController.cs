using System.Linq.Expressions;
using Onoicrm.Domain;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class RoleController :  BaseController
{
    
    public RoleController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager) : base(configuration, context, userManager)
    {
    }
    
    protected virtual IQueryable<IdentityRole> FilterPredicate(Filter filter, IQueryable<IdentityRole> entities)
    {

        switch (filter.Name)
        {   
            case "names":
            {
                var roleNames = filter.Value.GetString()!.Split($",").ToList(); 
                return entities.Where(ups => roleNames.Contains(ups.Name));
            }
            default: return entities;
        }
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
            .Filter(config.Filter, FilterPredicate);

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
}