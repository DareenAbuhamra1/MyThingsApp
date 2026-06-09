using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyThings.Core.Enums;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Helper;

public class JWTMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JWTMiddleware(RequestDelegate next,  IOptions < AppSettings > appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }
    public async Task Invoke(HttpContext context, IUserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (!string.IsNullOrWhiteSpace(token))
        {
            await AttachUserToContext(context, userService, token);
        }

        await _next(context);
    }

    private async Task AttachUserToContext(HttpContext context, IUserService userService, string token)
    {
        try
        {
            var TokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.Key));

            TokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true, 
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidIssuer = _appSettings.Issuer,
                ValidAudience = _appSettings.Audience,
                ClockSkew = TimeSpan.Zero,
            }, out SecurityToken validatedToken);

            var jwt = (JwtSecurityToken)validatedToken;

            var userIdClaim = jwt.Claims.FirstOrDefault(x =>
                x.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out var userId))
            {
                return;
            }

            var user = await userService.GetByIdAsync(userId);
            
            var tokenRole = jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            
            if (user is null)
            {
                // Fallback: If Read DB replication is lagging, construct a user from the trusted token claims
                if (!string.IsNullOrWhiteSpace(tokenRole) && Enum.TryParse<RoleEnum>(tokenRole, ignoreCase: true, out var parsedRole))
                {
                    user = parsedRole == RoleEnum.Customer ? new Customer { Id = userId, Role = parsedRole } : null;
                }
                
                if (user is null) return;
            }
            
            if (!string.IsNullOrWhiteSpace(tokenRole) &&
                Enum.TryParse<RoleEnum>(tokenRole, ignoreCase: true, out var role) &&
                _appSettings.AllowedRoles.Length > 0 &&
                !_appSettings.AllowedRoles.Contains(role.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                context.Items["ForbiddenRole"] = role.ToString();
                return;
            }

            context.Items["User"] = user;
        }
        catch (Exception)
        {
            // Invalid tokens are handled by the custom AuthorizeAttribute.
        }
    }
}
