using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Services;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Onoicrm.Api.Attributes;
using Onoicrm.Api.Controllers.Base.Public;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
[ValidateSecurityStamp]
public class BookingToothFileController : FileController<BookingToothFile>
{
    public BookingToothFileController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager,IFileService fileService) : base(configuration, dataContext,userManager,fileService)
    {
    }
    
    [HttpPost("upload/{bookingId:long}")]
    public async Task<IActionResult> UploadFile(long bookingToothId, IFormFile file)
    {
        var model = new BookingToothFile()
        {
            Extension = Path.GetExtension(file.FileName),
            Name = file.FileName,
            BookingToothId = bookingToothId,
            FileSize = file.Length
        };
        return await ExecuteRequest(async () => await SaveFile(model, file));
    }
}