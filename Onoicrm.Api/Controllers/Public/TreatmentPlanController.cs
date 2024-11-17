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
public class TreatmentPlanController : PublicObjectController<TreatmentPlan>
{
    public TreatmentPlanController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    protected override IQueryable<TreatmentPlan> FilterPredicate(Filter filter, IQueryable<TreatmentPlan> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        return filter.Name switch
        {
            "clinicId" => result.Where(ups => ups.ClinicId == filter.Value.GetInt32()),
            "doctorId" => result.Where(ups => ups.DoctorId == filter.Value.GetInt32()),
            "patientId" => result.Where(ups => ups.PatientId == filter.Value.GetInt32()),
            "toothId" => result.Where(ups => ups.ToothId == filter.Value.GetInt32()),
            _ => result
        };
    }
}