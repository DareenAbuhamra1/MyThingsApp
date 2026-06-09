using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;

namespace MyThings.Auth.AuthServices;

public class TokenService : ITokenService
{

    private readonly IConfiguration _configuration;
    private readonly ILogger<TokenService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    
    private DateTime Expiry => DateTime.UtcNow.AddHours(
       double.Parse(_configuration["Jwt:ExpiryHours"] ?? "2"));

    

    public TokenService(IConfiguration configuration,ILogger<TokenService> logger,IUnitOfWork unitOfWork)
    {
        _configuration =configuration;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public string GenerateJwtToken(User user, RoleEnum role)
    {
        // Metadata, Stored in the header; Jwt,SHA256

        // This creates a cryptographic key for the key I saved in appsettings
        var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        // Hash the key we creted using SHA-256
        var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);


        // Payload, Claims of the user and timestamps
        var Claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(ClaimTypes.Role, role.ToString()),
            new Claim("LastLogin", DateTime.UtcNow.ToString())
        };


        var Token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: Claims,
            expires: Expiry,
            signingCredentials: Credentials
        );

        // Lastly, The signature
        // WriteToke() converts C# object into an encoded string, and provide a Signature to ensure integrity
        return new JwtSecurityTokenHandler().WriteToken(Token);
    }
    public async Task<AuthResponseDto> RefreshTokenAsync(string expiredToken, string refreshToken)
    {
        try
        {
            var principal = GetClaimsPrincipalFromExpiredToken(expiredToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out int userIdInt))
            {
                _logger.LogWarning("Refresh failed: Could not parse UserId from expired token.");
                return new AuthResponseDto { IsSuccess = false, Message = "Invalid Token" };
            }

            var user = await _unitOfWork.Users.GetByIdAsync(userIdInt);
            if (user?.RefreshToken is null || user.RefreshTokenExpiry is null)
            {
                _logger.LogWarning("Refresh denied for User {UserId}: Refresh token is missing.", userIdInt);
                return new AuthResponseDto { IsSuccess = false, Message = "Session Expired" };
            }

            byte[] dbTokenBytes = Encoding.UTF8.GetBytes(user.RefreshToken);
            byte[] inputTokenBytes = Encoding.UTF8.GetBytes(refreshToken);

            bool isTokenValid = CryptographicOperations.FixedTimeEquals(dbTokenBytes, inputTokenBytes);
            bool isExpired = user.RefreshTokenExpiry <= DateTime.UtcNow;
            // Security Validation
            if (user == null || !isTokenValid || isExpired)
            {
                _logger.LogWarning("Refresh denied for User {UserId}: Token mismatch or expired.", userIdInt);
                return new AuthResponseDto { IsSuccess = false, Message = "Session Expired" };
            }

            var roleClaim = principal.FindFirst(ClaimTypes.Role);
            if (roleClaim == null || !Enum.TryParse(roleClaim.Value, out RoleEnum role))
            {
                return new AuthResponseDto { IsSuccess = false, Message = "Role missing" };
            }

            // Generate New Pair
            var newAccessToken = GenerateJwtToken(user, role);
            var newRefreshToken = GenerateRefreshToken();

            // Update DB (Rotation)
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(30);
            user.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                Expiry = Expiry,
                UserId = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Role = role
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during token refresh.");
            return new AuthResponseDto { IsSuccess = false, Message = "Internal Error" };
        }
    }
    public string GenerateRefreshToken()
    {
        var RandomNo = new byte[32];
        using var Range = System.Security.Cryptography.RandomNumberGenerator.Create();

        Range.GetBytes(RandomNo);

        return Convert.ToBase64String(RandomNo);
    }

    public ClaimsPrincipal GetClaimsPrincipalFromExpiredToken(string token)
    {
        // token: The raw JWT string sent by the Flutter app
        // to define the rules for what makes a token valid
        var TokenValidationParameters = new TokenValidationParameters
        {
            // keep it false for testing, in prod make it true
            ValidateAudience = false, // Set based on your config
            // If true, check issuer in appsettings if it doesn't match kick them out
            // Here it bypasses the check (in production it should be true)
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
            ValidateLifetime = false // CRITICAL: This allows reading the claims of an expired token
        };

        //You use this when you need to perform an action on a token string (like reading it or creating it).

        var TokenHandler = new JwtSecurityTokenHandler();

        //If the signature matches your secret key, it returns this object containing all the user's claims (ID, Role, etc.).
        //The method "fills up" the securityToken variable with the technical object representing the JWT.
        var principal = TokenHandler.ValidateToken(token, TokenValidationParameters, out SecurityToken securityToken);

        // Security Check: Ensure the token was signed with the expected algorithm (HmacSha256)
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token signature algorithm.");
        }

        return principal;
    }
    
}
