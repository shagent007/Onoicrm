using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/public/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class PaymentController : PublicObjectController<Payment>
{
    public PaymentController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }
    
    protected override IQueryable<Payment> FilterPredicate(Filter filter, IQueryable<Payment> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "profileId": return result.Where(ups => ups.ProfileId == filter.Value.GetInt32());
            case "patientId": return result.Where(ups => ups.PatientId == filter.Value.GetInt32());
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt32());
            case "method": return result.Where(ups => ups.Method == filter.Value.GetString());
            default: return result;
        }
    }
}