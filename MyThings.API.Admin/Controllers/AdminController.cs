using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Helper;

namespace AdminController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly WriteDbContext _context;
        private readonly ILogger<AdminController> _logger;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AdminController(WriteDbContext context, ILogger<AdminController> logger
        , IAuthService authService,ITokenService tokenService )
        {
            _context = context;
            _logger = logger;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("auth/login-admin")]
        public async Task<IActionResult> LoginAdmin([FromBody] AdminLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var loginResult = await _authService.LoginAdminAsync(loginDto);

                if (loginResult.IsSuccess == false)
                {
                    _logger.LogWarning(loginResult.Message);
                    return Unauthorized(new { Message = loginResult.Message });
                }
                _logger.LogInformation(loginResult.Message);
                return Ok(loginResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during admin login");
                return StatusCode(500, new { Message = "An internal error occurred during login." });
            }
        }
        [HttpPost("auth/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var refreshTokenResult = await _tokenService.RefreshTokenAsync(
                refreshTokenDto.ExpiredToken, refreshTokenDto.RefreshToken);

                return Ok(refreshTokenResult);
            }
            catch (SecurityTokenException ex) // Catch specific security errors
            {
                _logger.LogInformation(ex.Message);
                // This tells the Flutter app: "Your session is fully expired. Go to Login screen."
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return StatusCode(500, new { Message = "An internal error occurred during token refresh." });
            }
        }
        [Authorize(RoleEnum.SuperAdmin,RoleEnum.Admin)]
        [HttpPost("auth/logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                {
                    return Unauthorized(new { Message = "Missing or invalid authorization header" });
                }

                var token = authHeader.Substring("Bearer ".Length).Trim();
                
                var principal = _tokenService.GetClaimsPrincipalFromExpiredToken(token);
                var userClaimId = principal.FindFirst(ClaimTypes.NameIdentifier);

                if (userClaimId == null || !int.TryParse(userClaimId.Value, out var userId))
                {
                    return BadRequest(new {Message = "Invalid user identification claims context"});
                }
                
                var result = await _authService.RevokeSessionAsync(userId);

                if(!result.Success) return StatusCode(result.StatusCode, new {Message = result.Message});

                return Ok(result.Data);
            }
            catch(SecurityTokenException ex)
            {
                _logger.LogWarning("Logout failed due to invalid token signature: {Message}", ex.Message);
                return Unauthorized(new { Message = "Invalid token" });
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred during logout");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
    }
}
