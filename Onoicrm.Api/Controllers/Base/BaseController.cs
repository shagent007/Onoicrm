using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;

namespace Onoicrm.Api.Controllers.Base;

[ApiController]
[Route("[controller]")]

public abstract class BaseController : ControllerBase
{
    protected readonly ApplicationDataContext Context;
    protected readonly IConfiguration Configuration;
    protected readonly UserManager<IdentityUser> UserManager;

    protected BaseController(IConfiguration configuration, ApplicationDataContext context, UserManager<IdentityUser> userManager)
    {
        Configuration = configuration;
        Context = context;
        UserManager = userManager;
    }

    protected async Task<IdentityUser> GetUser(string? userName = null)
    {
        userName ??= User.Identity?.Name ?? string.Empty;
        var user = await UserManager.FindByNameAsync(userName);
        if (user == null) throw new Exception("Пользователь не найден");
        return user;
    }
    protected async Task<UserProfile> GetUserProfile(IdentityUser? user = null)
    {
        user ??= await GetUser();
        var userProfile = await Context.Set<UserProfile>().FirstOrDefaultAsync(up => up.UserId == user.Id);
        if (userProfile == null) throw new NullReferenceException("Профиль пользователя не найден");
        return userProfile;
    }
    protected async Task<IActionResult> ExecuteRequest<T>(Func<Task<T>> func)
    {
        try
        {
            var result = await func();
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    protected async Task<IActionResult> ExecuteRequest(Func<Task> func)
    {
        try
        {
            await func();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    protected async Task ExecuteDbCommand(Func<Task> function)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            await function();
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception(e.Message);
        }
    }
    protected async Task<T> ExecuteDbCommand<T>(Func<Task<T>> function)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            var result = await function();
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
            return result;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception(e.Message);
        }
    }
    protected async Task ExecuteDbCommand(Action function)
    {
        await using var transaction = await Context.Database.BeginTransactionAsync();
        try
        {
            function();
            await Context.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            throw new Exception(e.Message);
        }
    }
}