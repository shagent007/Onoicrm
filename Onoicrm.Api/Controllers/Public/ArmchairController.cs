using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;


[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/public/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class ArmchairController : PublicObjectController<Armchair>
{
    public ArmchairController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    protected override IQueryable<Armchair> FilterPredicate(Filter filter, IQueryable<Armchair> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt64());
            default: return result;
        }
    }
}