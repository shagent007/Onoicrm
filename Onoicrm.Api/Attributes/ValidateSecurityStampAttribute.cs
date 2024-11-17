using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Onoicrm.Api.Attributes;

public class ValidateSecurityStampAttribute : AuthorizeAttribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;
        if (user.Identity is { IsAuthenticated: true, Name: not null})
        {
            var userManager = (UserManager<IdentityUser>)context.HttpContext.RequestServices.GetService(typeof(UserManager<IdentityUser>));
            var securityStamp = user.FindFirstValue(ClaimTypes.UserData);

            if (!string.IsNullOrEmpty(securityStamp))
            {
                var userInDb = await userManager?.FindByEmailAsync(user.Identity.Name)!;

                if (userInDb == null || userInDb.SecurityStamp != securityStamp)
                {
                    var result = new ObjectResult("AuthDuplicateError")
                    {
                        StatusCode = 403
                    }; 
                    context.Result = result;
                }
            }
        }
    }
}