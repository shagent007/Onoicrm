using Onoicrm.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain.Services;

namespace Onoicrm.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            var res = await _userService.LoginUserAsync(model);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
  
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        try
        {
            var res = await _userService.RegisterUserAsync(model);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
    {
        try
        {
            var res = await _userService.ResetPasswordAsync(model);
            return Ok(res);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
}