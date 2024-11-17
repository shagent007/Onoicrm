using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Services;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base;

namespace Onoicrm.Api.Controllers.Admin;

[Route("api/v1/admin/[controller]")]
[ApiController]
[Authorize(Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class UserRoleController :  BaseController
{
    private readonly IUserService _userService;
    
    public UserRoleController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager, IUserService userService) : base(configuration, context, userManager)
    {
        _userService = userService;
    }

    [HttpPost("set-role")]
    public async Task<IActionResult> SetRole([FromBody] SetRoleModel model)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(async () =>
            {
                await _userService.SetRoleAsync(model);
            });
        });
    }
    
    [HttpPost("delete-role")]
    public async Task<IActionResult> DeleteRole([FromBody] SetRoleModel model)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(async () =>
            {
                await _userService.DeleteRoleAsync(model);
            });
        });
    }

}