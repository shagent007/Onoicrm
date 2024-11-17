using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ValidateSecurityStamp]
public class ClinicController : PublicObjectController<Clinic>
{

    public ClinicController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext, userManager)
    {
    }
    

    [HttpGet("{id:long}")]
    public override async Task<IActionResult> GetById(long id) => await ExecuteRequest(async () =>
    {
        var model = await Context.Set<Clinic>()
            .FirstOrDefaultAsync(c => c.Id == id );

        if (model == null) throw new Exception("У вас нет доступ к данной клинике");
        return model;
    });
    
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



}