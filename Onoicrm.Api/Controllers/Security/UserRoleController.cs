using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Exceptions;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Security;

[Route("api/v1/security/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class UserRoleController : BaseController
{

    private readonly RoleManager<IdentityRole> _roleManager;

    public UserRoleController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager) : base(configuration, context, userManager)
    {
        _roleManager = roleManager;
    }

    [HttpPost("set-role")]
    public async Task<IActionResult> SetRole([FromBody] SetRoleModel model) => await ExecuteRequest(async () =>
    {
        return await ExecuteDbCommand(async () =>
        {
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null) throw new NullReferenceException();
            var isExist = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!isExist) throw new NullReferenceException("Такой роли не сушествует");
            var result = await UserManager.AddToRoleAsync(user, model.RoleName);
            if (!result.Succeeded) throw new UserRegistrationException(result.Errors);
            return result;
        });
    });


    [HttpPost("delete-role")]
    public async Task<IActionResult> DeleteRole([FromBody] SetRoleModel model) => await ExecuteRequest(async () =>
    {
        return await ExecuteDbCommand(async () =>
        {
            var user = await UserManager.FindByIdAsync(model.UserId);
            if (user == null) throw new NullReferenceException();
            var isExist = await _roleManager.RoleExistsAsync(model.RoleName);
            if (!isExist) throw new NullReferenceException("Такой роли не сушествует");
            var result = await UserManager.RemoveFromRoleAsync(user, model.RoleName);
            if (!result.Succeeded) throw new UserRegistrationException(result.Errors);
            return result;
        });
    });
}