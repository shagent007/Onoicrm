using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Api.Attributes;

namespace Onoicrm.Api.Controllers.Base.Admin;

[Route("api/v1/admin/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public abstract class GroupController<TGroup> : AdminObjectController<TGroup> where TGroup : Group, new()
{
    
    protected GroupController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }

    [HttpGet("{name}/tree")]
    public async Task<IActionResult> GetTree(string name) => await ExecuteRequest(async () =>
    {
        var model = await GetModel(m => m.Name == name);
        if (model == null) throw new NullReferenceException($"Обьект не найден");
        LoadChildren(model);
        return model;
    });
    
 
  

    protected virtual void LoadChildren(TGroup model)
    {
        Context.Entry(model).Collection(c => c.Children).Load();
        foreach (var child in model.Children)
        {
            LoadChildren((TGroup)child);
        }
    }
}