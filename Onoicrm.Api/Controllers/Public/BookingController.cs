using System.Linq.Expressions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Services;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookingController : PublicObjectController<Booking>
{
    private readonly IFileService _fileService;
    private readonly WappiService _wappiService;
    public BookingController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager, IFileService fileService, WappiService wappiService) : base(configuration, dataContext,userManager)
    {
        _fileService = fileService;
        _wappiService = wappiService;
    }
    
 

    protected override async Task<Booking> GetModel(Expression<Func<Booking, bool>> expression)
    {
        var model = await Context.Set<Booking>()
            .AsNoTracking()
            .Include(b => b.ImplementedServices)
            .AsSplitQuery()
            .Include(b=> b.Patient)
            .AsSplitQuery()
            .Include(b => b.Doctor)
            .AsSplitQuery()
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.ImplementedServices)
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.Files)
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.Channels)
            .Include(b => b.Files)
            .SingleAsync(expression);
        
        return model;
    }

    [HttpPut("{id:long}/report")]
    public async Task<IActionResult> UpdateReport([FromBody]BookingReportModel model, long id)
    {
        return await ExecuteRequest(async () =>
        {
            await ExecuteDbCommand(async () =>
            {
                if (model.ForDeleteBookingToothFiles.Any())
                {
                    foreach (var file in model.ForDeleteBookingToothFiles)
                    {
                        _fileService.Delete(file);
                    }
                }
                if (model.ForDeleteBookingFiles.Any())
                {
                    foreach (var file in model.ForDeleteBookingFiles)
                    {
                        _fileService.Delete(file);
                    }
                }
                
                //Remove
                Context.Set<ImplementedService>().RemoveRange(model.ForDeleteImplementedServices);
                Context.Set<BookingTooth>().RemoveRange(model.ForDeleteBookingTeeth);
                await Context.SaveChangesAsync();
                
                //Add
                if (model.ForAddBookingFiles.Any())
                {
                    foreach (var file in model.ForAddBookingFiles.Where(file => file.Base64.IsBase64String()))
                    {
                        await _fileService.Save(file, file.Base64);
                    }
                }
                if (model.ForAddBookingTeeth.Any())
                {
                    foreach (var bt in model.ForAddBookingTeeth)
                    {
                        foreach (var file in bt.Files)
                        {
                            if(string.IsNullOrEmpty(file.Base64))continue;
                            if(!file.Base64.IsBase64String()) continue;
                            await _fileService.Save(file, file.Base64);
                        }
                        Context.Set<BookingTooth>().Add(bt);
                    }
                }
                if (model.ForAddImplementedServices.Any())
                {
                    Context.Set<ImplementedService>().AddRange(model.ForAddImplementedServices);
                }
                
                // Update
                if (model.ForUpdateBookingTeeth.Any())
                {
                    foreach (var bt in model.ForUpdateBookingTeeth)
                    {
                        foreach (var file in bt.Files)
                        {
                            if(string.IsNullOrEmpty(file.Base64))continue;
                            if(!file.Base64.IsBase64String()) continue;
                            if (Context.Set<BookingToothFile>().Any(f => f.Id == file.Id))
                            {
                                 _fileService.Update(file, file.Base64);
                            }
                            else
                            {
                                await _fileService.Save(file, file.Base64);
                            }
                        }   
                        Context.Set<BookingTooth>().Update(bt);
                    }
                }
            });

            var result = await GetModel(b=> b.Id == id);
    
            result.LastUpdate = DateTime.Now.ToUniversalTime();
            
            return result;
        });
    }
    
    [HttpPost("{id:long}/upload")]
    public async Task<IActionResult> UploadFile(long id, IFormFile file)
    {
        return await ExecuteRequest(async () =>
        {
            return await ExecuteDbCommand(async () =>
            {
                var model = new BookingFile
                {
                    Extension = Path.GetExtension(file.FileName),
                    Name = file.FileName,
                    BookingId = id,
                    FileSize = file.Length,
                };
                return await _fileService.Save(model, file);
            });
        });
    }

    [HttpGet("schedule/{clinicId:long}/{from},{to}")]
    public async Task<IActionResult> GetSchedule(long clinicId, string from, string to, long? doctorId, long? armchairId)
    {
        try
        {
            
            var fromDate = from.TryParseDateTimeOffset();
            var toDate = to.TryParseDateTimeOffset(); 
            
            var query = Context.Set<Booking>()
                .AsNoTracking()
                .Where(b => 
                    b.ClinicId == clinicId 
                    &&
                    b.StateId != StateNames.Canceled
                    &&
                    b.DateTimeStart >= fromDate && b.DateTimeStart <= toDate
                );

            if (doctorId != null)
            {
                query = query.Where(b => b.DoctorId == doctorId);
            }

            if (armchairId != null)
            {
                query = query.Where(b => b.ArmchairId == armchairId);
            }
            
            var items  = await query     
                .Include(b =>b.Patient)
                .Include(b=>b.Doctor)
                .Include(b => b.ServiceGroup)
                .Select(b => new
                {
                    b.Id,
                    b.StateId,
                    Start = b.DateTimeStart,
                    End = b.DateTimeEnd,
                    BackgroundColor = b.ServiceGroup.BgColor,
                    BorderColor = b.ServiceGroup.TextColor,
                    Patient = b.Patient != null ? new
                    {
                        b.Patient.FullName, b.Patient.Id
                    } : null,
                    Doctor = b.Doctor != null ? new
                    {
                        b.Doctor.FullName, b.Doctor.Id
                    } : null,
                    b.ArmchairId,
                    b.Caption
                })
                .Paginate(1, 100)
                .ToListAsync();
            
            return Ok(items);
        }
        catch (Exception e)
        {
            return BadRequest(new ErrorResponse(e));
        }
    }
    
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public override async Task<IActionResult> Add(Booking model) => await ExecuteRequest(async () =>
    {
        var group = await Context.Set<BookingGroup>().FirstOrDefaultAsync(bg => bg.ClinicId == model.ClinicId && bg.PatientId == model.PatientId && bg.StateId == StateNames.Activated);
        
        if (group == null)
        {
            group = new BookingGroup
            {
                ClinicId = model.ClinicId,
                PatientId = model.PatientId,
                StateId = StateNames.Activated,
                StartDate = model.DateTimeStart
            };
        
            Context.Set<BookingGroup>().Add(group);
            await Context.SaveChangesAsync();
        }
        
        model.BookingGroupId = group.Id;
        if(model.PatientId != null)
        {
            model.Patient = null;
        }
        
        await ExecuteDbCommand(async () => await Context.Set<Booking>().AddAsync(model));
        var result = await GetModel(m => m.Id==model.Id);
        return result;
    });
    
    [HttpPost("{id:long}/notify/doctor")]
    public async Task<IActionResult> NotifyDoctor(long id, [FromBody] string text)
    {
        var model = await Context.Set<Booking>()
            .Include(b => b.Clinic)
            .Include(b => b.Doctor)
            .Include(b => b.Patient)
            .Include(b => b.Armchair)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (model == null) throw new Exception("Запись не найден");
        
        await _wappiService.SendMessageAsync(new WhatsAppMessage
        {
            To = model.Doctor?.WhatsAppNumber.ClearPhoneNumber(),
            Text = text.ReplaceTemplate(model),
            ProfileId = model.Clinic?.WappiProfileId,
            Token = model.Clinic?.WappiToken
        });

        return Ok();
    }
    
    [HttpPost("{id:long}/notify/patient")]
    public async Task<IActionResult> NotifyPatient(long id, [FromBody] string text)
    {
        var model = await Context.Set<Booking>()
            .Include(b => b.Clinic)
            .Include(b => b.Doctor)
            .Include(b => b.Patient)
            .Include(b => b.Armchair)
            .FirstOrDefaultAsync(d => d.Id == id);

        if (model == null) throw new Exception("Запись не найден");

     
        await _wappiService.SendMessageAsync(new WhatsAppMessage
        {
            To = model.Patient?.WhatsAppNumber.ClearPhoneNumber(),
            Text = text.ReplaceTemplate(model),
            ProfileId = model.Clinic?.WappiProfileId,
            Token = model.Clinic?.WappiToken
        });
    

        return Ok();
    }
    
    [HttpDelete("{id:long}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
    public override async Task<IActionResult> Delete(long id) => await ExecuteRequest(async () => await ExecuteDbCommand(async () =>
    {
        var model =await Context.Set<Booking>()
            .Include(b => b.Files)
            .Include(b => b.ImplementedServices)
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.ImplementedServices)
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.Files)
            .Include(b => b.BookingTeeth)
            .ThenInclude(bt => bt.Channels)
            .SingleAsync(m=>m.Id==id);
        
        foreach (var bookingTooth in model.BookingTeeth)
        {
            foreach (var file in bookingTooth.Files)
            {
                _fileService.Delete(file);
            }
        }

        foreach (var file in model.Files)
        {
            _fileService.Delete(file);
        }
        await BeforeDelete(model);
        Context.Set<Booking>().Remove(model);
    }));
    
    protected override IQueryable<Booking> FilterPredicate(Filter filter, IQueryable<Booking> entities)
    {
        var result = base.FilterPredicate(filter, entities)
            .AsNoTracking()
            .Include(b => b.Patient)
            .AsSplitQuery()
            .Include(b => b.Doctor);

        switch (filter.Name)
        {
            case "clinicId": return result.Where(ups => ups.ClinicId == filter.Value.GetInt64() || ups.ClinicId == null);
            case "patientId": return result.Where(ups => ups.PatientId == filter.Value.GetInt64());
            case "doctorId": return result.Where(ups => ups.DoctorId == filter.Value.GetInt64());
            case "armchairId": return result.Where(ups => ups.ArmchairId == filter.Value.GetInt64());
            case "stateId": return result.Where(ups => ups.StateId == filter.Value.GetInt64());
            case "groupId": return result.Where(ups => ups.BookingGroupId == filter.Value.GetInt64());
            case "dateTime":
            {
                var dateTime = filter.TryParseDateTimeOffset();
                return result.Where(ups => ups.DateTimeStart == dateTime);
            }
            case "exclude":
            {
                var excludeIds = filter.Value.GetString()
                    !.Split(',')
                    .Select(x => long.Parse(x)!);
                return result.Where(b => !excludeIds.Contains(b.Id));
            }
            case "dateTimeRange":
            {
                var dateParts = filter.Value.GetString()?.Split(',');
                if (dateParts == null) throw new NullReferenceException();
                var fromDate = dateParts[0].TryParseDateTimeOffset();
                var toDate = dateParts[1].TryParseDateTimeOffset(); 
                return result.Where(b => b.DateTimeStart >= fromDate && b.DateTimeStart <= toDate);
            }
            case "cancellationReasonId": return result.Where(b => Context
                .Set<BookingCancellationReason>()
                .Any(bcr =>
                    bcr.CancellationReasonId == filter.Value.GetInt32()
                    &&
                    bcr.BookingId == b.Id
                )
            );
            case "notCancelled":return result.Where(b => !Context
                .Set<BookingCancellationReason>()
                .Any(bcr => bcr.BookingId == b.Id)
            );
            case "cancelled": return result.Where(b => Context
                .Set<BookingCancellationReason>()
                .Any(bcr => bcr.BookingId == b.Id)
            );
            default: return result;
        }
    }
}