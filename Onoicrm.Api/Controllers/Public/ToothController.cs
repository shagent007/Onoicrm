using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class ToothController : PublicObjectController<Tooth>
{
    public ToothController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    [HttpGet]
    public override async Task<IActionResult> GetList(int pageIndex=1, int pageSize=20, string orderFieldName="Id", string orderFieldDirection="ASC", string filter = "", string fields="")
    {
        var config = new ListConfig(pageIndex, pageSize, orderFieldName, orderFieldDirection, filter);
        var query = Context.Set<Tooth>()
            .Include(t => t.Channels)
            .Filter(config.Filter, FilterPredicate)
            .Sort(config.OrderFieldName, config.OrderFieldDirection);

        var items = await query.Paginate(config.PageIndex, config.PageSize).ToListAsync();
        var total = query.Count();
        var result = new ListResponse(items, total);
        return Ok(result);
    }
}