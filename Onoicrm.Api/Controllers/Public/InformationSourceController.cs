using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Admin;

namespace Onoicrm.Api.Controllers.Public;

[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/public/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class InformationSourceController : AdminObjectController<InformationSource>
{
    public InformationSourceController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
}