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

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class ServiceGroupController: GroupController<ServiceGroup>
{
    [HttpGet("{clinicId:long}/root")]
    public async Task<IActionResult> GetClinicRootTree(long clinicId) => await ExecuteRequest(async () =>
    {
        var model = await GetModel(m => m.ClinicId == clinicId && m.ParentId == null);
        if (model == null) return null;
        LoadChildren(model);
             return model;
    });
    
    protected override void LoadChildren(ServiceGroup model)
    {
        Context.Entry(model).Collection(c => c.Children).Load();
        foreach (var child in model.Children)
        {
            LoadChildren((ServiceGroup)child);
        }
    }
    
    public ServiceGroupController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
}