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
public class DoctorServiceGroupSalaryController : PublicObjectController<DoctorServiceGroupSalary>
{
  public DoctorServiceGroupSalaryController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
  {
  }
    
  protected override IQueryable<DoctorServiceGroupSalary> FilterPredicate(Filter filter, IQueryable<DoctorServiceGroupSalary> entities)
  {
    var result = base.FilterPredicate(filter, entities);

    switch (filter.Name)
    {
      case "doctorId": return result.Where(ups => ups.DoctorId == filter.Value.GetInt32());
      case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt32());
      case "serviceGroupId": return result.Where(ups => ups.ServiceGroupId == filter.Value.GetInt32());
      default: return result;
    }
  }
}