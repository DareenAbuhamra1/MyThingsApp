using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MyThings.Core.Entities;
using MyThings.Core.Enums;

namespace MyThings.Infrastructure.Helper;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly RoleEnum[] _roles;

    public AuthorizeAttribute(params RoleEnum[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var authHeader = context.HttpContext.Request.Headers["Authorization"]
        .FirstOrDefault();

        var auth2 = context.HttpContext.Request.Headers.Authorization.ToString();

        if (context.HttpContext.Items.ContainsKey("ForbiddenRole"))
        {
            context.Result = new JsonResult(new
            {
                message = "Forbidden"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
            return;
        }

        var user = context.HttpContext.Items["User"] as User;
        var user2 = context.HttpContext.User;
        
        if(user is null)
        {
            context.Result = new JsonResult( new {
                message = "Unauthorized"
            })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
            return;
        }
        //[Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        if (_roles.Length > 0 && !_roles.Contains(user.Role))
        {
            context.Result = new JsonResult(new
            {
                message = "Forbidden"
            })
            {
                StatusCode = StatusCodes.Status403Forbidden
            };
        }
    }
}
