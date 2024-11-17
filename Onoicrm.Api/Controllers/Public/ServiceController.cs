using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class ServiceController : PublicObjectController<Service>
{
    public ServiceController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }

  
    
    protected override IQueryable<Service> FilterPredicate(Filter filter, IQueryable<Service> entities)
    {
        var result = base.FilterPredicate(filter, entities)
            .Include(s => s.Links)
            .Where(s => s.StateId == StateNames.Activated);

        
        
        switch (filter.Name)
        {
            case "searchText": return result.Where(s =>
                EF.Functions.Like(s.Caption.ToUpper(), $"%{filter.Value.GetString()}%".ToUpper())
                ||
                EF.Functions.Like(s.Description.ToUpper(), $"%{filter.Value.GetString()}%".ToUpper())
            );
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt32());
            case "groupId": return result.Where(s => Context.Set<ServiceGroupLink>()
                .Any(sgl => 
                    sgl.ServiceGroupId == filter.Value.GetInt32()
                    &&
                    sgl.ServiceId == s.Id
                )
            );
            default: return result;
        }
    }
}