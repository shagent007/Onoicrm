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


[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/public/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = $"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class BookingCancellationReasonController : PublicObjectController<BookingCancellationReason>
{
    public BookingCancellationReasonController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration, dataContext,userManager)
    {
    }

    [HttpPost("range")]
    public override async Task<IActionResult> AddRange([FromBody] List<BookingCancellationReason> items)
    {
       return await ExecuteRequest(async () =>
       {
           await ExecuteDbCommand(() => Context.Set<BookingCancellationReason>().AddRange(items));
           return items;
       });
    }

    [HttpGet("{clinicId}/statistics")]
    public async Task<IActionResult> GetStatistic(long clinicId) => await ExecuteRequest(async () =>
    {
        return await Context.Set<BookingCancellationReason>()
            .Where(bcr => bcr.ClinicId == clinicId)
            .Include(bcr => bcr.CancellationReason)
            .GroupBy(bcr => bcr.CancellationReasonId)
            .Select(s => new
            {
                Id = s.Key,
                Count = s.Count(),
                s.First().CancellationReason.Caption
            })
            .ToListAsync();
    });

  
    
    protected override IQueryable<BookingCancellationReason> FilterPredicate(Filter filter, IQueryable<BookingCancellationReason> entities)
    {
        var result = base.FilterPredicate(filter, entities);

        switch (filter.Name)
        {
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt64());
            default: return result;
        }
    }
}