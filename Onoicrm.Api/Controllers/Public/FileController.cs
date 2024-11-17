using Onoicrm.Domain;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Onoicrm.Api.Controllers.Public;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles =$"{UserRoles.SystemAdministrator}, {UserRoles.Director}, {UserRoles.SiteAdministrator}, {UserRoles.Administrator}, {UserRoles.Dentist}")]
public  class FileController : ControllerBase
{
    private readonly IFileService _fileService;
    
    public FileController(IFileService fileService) 
    {
        _fileService = fileService;
    }

    [HttpGet("{id:long}")]
    [AllowAnonymous]
    public virtual async Task<IActionResult> GetImage(long? id, bool download = false) =>
       await _fileService.Get<AttachedFile>(id, download);
}