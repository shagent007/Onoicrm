using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class SymptomController : PublicObjectController<Symptom>
{
    public SymptomController(IConfiguration configuration, ApplicationDataContext dataContext,
        UserManager<IdentityUser> userManager) : base(configuration, dataContext, userManager)
    {
    }


    protected override IQueryable<Symptom> FilterPredicate(Filter filter, IQueryable<Symptom> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt64() || ups.ClinicId == null);
            default: return result;
        }
    }
}