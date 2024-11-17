using 
    System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Admin;

namespace Onoicrm.Api.Controllers.Admin;

[Route("api/v1/admin/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class ClinicController : AdminObjectController<Clinic>
{
    public ClinicController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }

    protected override IQueryable<Clinic> FilterPredicate(Filter filter, IQueryable<Clinic> entities)
    {
        return filter.Name switch
        {
            "id" => entities.Where(e => e.Id == filter.Value.GetInt64()),
            "caption" => entities.Where(e =>
                EF.Functions.Like(e.Caption.ToUpper(),$"%{filter.Value.GetString()}%".ToUpper())),
            _ => entities
        };
    }

    protected override async Task<Clinic> GetModel(Expression<Func<Clinic, bool>> expression)
    {
        var model = await Context.Set<Clinic>()
            .SingleAsync(expression);
    

        return model;
    }


    public override async Task<IActionResult> Add(Clinic model)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(async () =>
            {
                Context.Set<Clinic>().Add(model);
                await Context.SaveChangesAsync();
                var serviceGroup = new ServiceGroup
                {
                    Caption = "Все усулги",
                    ClinicId = model.Id
                };
                Context.Set<ServiceGroup>().Add(serviceGroup);
            });
            var result = await GetModel(m => m.Id == model.Id);
            return result;
        });
    }
}