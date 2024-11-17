using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Services;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;


[ApiController]
[ValidateSecurityStamp]
[Route("api/v1/public/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public class BookingToothController : PublicObjectController<BookingTooth>
{
    private readonly IFileService _fileService;

    public BookingToothController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager, IFileService fileService) : base(configuration, dataContext,userManager)
    {
        _fileService = fileService;
    }
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public override async Task<IActionResult> Add(BookingTooth model) => await ExecuteRequest(async () =>
    {
        await ExecuteDbCommand(async () =>
        {
            foreach (var file in model.Files)
            {
                if (!file.Base64.IsBase64String()) throw new Exception($"Файл под названием, ${file.Name} повреждён");
                var data = Convert.FromBase64String(file.Base64);
            }
            await Context.Set<BookingTooth>().AddAsync(model);
        });
        var result = await GetModel(m => m.Id==model.Id);
        return result;
    });
    
    
    [HttpPost("{id:long}/upload")]
    public async Task<IActionResult> UploadFile(long id, IFormFile file)
    {
        return await ExecuteRequest(async () =>
        {
            var model = new BookingToothFile
            {
                Extension = Path.GetExtension(file.FileName),
                Name = file.FileName,
                BookingToothId = id,
                FileSize = file.Length,
            };

            return await _fileService.Save(model, file);
        });
    }

    protected override async Task<BookingTooth> GetModel(Expression<Func<BookingTooth, bool>> expression)
    {
        var model = await Context.Set<BookingTooth>()
            .Include(bt => bt.ToothState)
            .Include(bt => bt.Files)
            .SingleAsync(expression);
        return model;    
    }


    protected override IQueryable<BookingTooth> FilterPredicate(Filter filter, IQueryable<BookingTooth> entities)
    {
        var result = base.FilterPredicate(filter, entities)
            .Include(b => b.Booking)
            .ThenInclude(b=>b.Patient)
            .Include(b => b.Booking)
            .ThenInclude(b=>b.Doctor);
        

        switch (filter.Name)
        {
            case "toothId": return result.Where(r => r.ToothId == filter.Value.GetInt32());
            case "bookingId": return result.Where(r => r.BookingId == filter.Value.GetInt32());
            case "patientId": return result.Where(r => r.Booking.PatientId == filter.Value.GetInt32());
            
            default: return result;
        }
    }
}