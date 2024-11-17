using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Admin;

namespace Onoicrm.Api.Controllers.Admin;

[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/admin/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class ChannelController : AdminObjectController<Channel>
{
    public ChannelController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    protected override IQueryable<Channel> FilterPredicate(Filter filter, IQueryable<Channel> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "toothId": return result.Where(ups => ups.ToothId == filter.Value.GetInt64());
            default: return result;
        }
    }
}