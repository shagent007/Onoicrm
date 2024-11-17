using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Services;
using Onoicrm.DataContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Onoicrm.Api.Controllers.Base.Public;

[Route("api/v1/public/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public abstract class FileController<TFileClass> : PublicObjectController<TFileClass> where TFileClass : AttachedFile, new()
{

    private readonly IFileService _fileService;
    
    protected FileController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager, IFileService fileService) : base(configuration, dataContext,userManager)
    {
        _fileService = fileService;
    }
    

    protected async Task SaveFile(TFileClass model, IFormFile file)
    {
        await ExecuteDbCommand(async () => await _fileService.Save(model, file));
    }


    [HttpGet("{id:long}/image.jpg")]
    [AllowAnonymous]
    public virtual async Task<IActionResult> GetImage(long? id, bool download = false) =>
       await _fileService.Get<TFileClass>(id, download);
}