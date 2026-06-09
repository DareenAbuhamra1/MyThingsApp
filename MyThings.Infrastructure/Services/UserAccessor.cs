using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Services;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public int GetCurrentUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(int.TryParse(userId, out var id))
            return id;

        throw new UnauthorizedAccessException("User ID not found in token");
    }

    public string GetCurrentUserRole()
    {
        return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value??"Guest";
    }
}