using System.Security.Claims;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;

namespace MyThings.Core.Interfaces;

public interface ITokenService
{
    string GenerateJwtToken(User user, RoleEnum role);
    string GenerateRefreshToken();
    ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token);
    Task<AuthResponseDto> RefreshTokenAsync(string expiredToken, string refreshToken);
    

}