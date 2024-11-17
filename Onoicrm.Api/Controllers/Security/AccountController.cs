using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.DataContext;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Security;

[Route("api/v1/security/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController : BaseController
{
    public AccountController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager) : base(configuration, context, userManager)
    {
    }


    [HttpGet]
    public virtual async Task<IActionResult> GetProfile() => await ExecuteRequest(async () =>
    {
        
        var user = await GetUser();
        var profile = await GetUserProfile(user);
        profile.Roles = await UserManager.GetRolesAsync(user);
        
        return profile;
    });
}