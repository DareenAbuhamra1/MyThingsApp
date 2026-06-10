using System.Security.Claims;
using System.ServiceModel.Channels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Helper;

namespace Controllers.CustomerController
{

    [Route("api/[controller]")]
    [ApiController]

    public class CustomerController : ControllerBase
    {

        private readonly ILogger<CustomerController> _logger;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public CustomerController(ILogger<CustomerController> logger, IAuthService authService, ITokenService tokenService)
        {
            _logger = logger;
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpPost("auth/register-customer")]
        public async Task<IActionResult> RegisterCustomer(RegisterRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("RegisterCustomer failed model validation. Request payload might be invalid or missing required fields.");
                return BadRequest(ModelState);
            }
            try
            {
                var registerRequest = await _authService.RegisterCustomerAsync(request);

                if (!registerRequest.IsSuccess)
                {
                    _logger.LogWarning("Customer registration failed for {Phone}: {Message}", request.Phone, registerRequest.Message);
                    return BadRequest(new { Message = registerRequest.Message });
                }
                _logger.LogInformation("Customer {Phone} registered successfully.", request.Phone);
                return Ok(registerRequest);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while registering a new customer with phone {Phone}.", request.Phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        [HttpPost("auth/request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] string phone)
        {
            try
            {

                int isValid = await _authService.RequestOtpAsync(phone);

                if (isValid != -1)
                {
                    _logger.LogInformation("OTP successfully sent to {Phone}.", phone);
                    return Ok(new { Message = "OTP sent successfully", Otp = isValid });
                }

                _logger.LogWarning("Failed to send OTP to {Phone}. Service returned invalid.", phone);
                return BadRequest("Could not send OTP. Please check the phone number and try again.");
            }
            catch (Exception e)
            {

                _logger.LogError(e, "An unexpected error occurred while requesting OTP for {Phone}.", phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        [HttpPost("auth/verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto verifyOtp)
        {
            try
            {

                var authResult = await _authService.VerifyOtpAsync(verifyOtp);

                if (!authResult.IsSuccess)
                {
                    _logger.LogWarning("Verification failed for {Phone}: {Message}", verifyOtp.Phone, authResult.Message);
                    return Unauthorized(new { Message = authResult.Message });
                }
                if (authResult.IsRegistered)
                {
                    if (authResult.Role != RoleEnum.Customer)
                    {
                        _logger.LogWarning("Access denied for {Phone}: Not a customer account.", verifyOtp.Phone);
                        return StatusCode(403, new { message = "Access denied. Not a customer account." });
                    }

                    _logger.LogInformation("Customer {Phone} verified correctly.", verifyOtp.Phone);
                    return Ok(authResult);
                }
                else
                {

                    _logger.LogInformation("New user {Phone} verified; moving to registration.", verifyOtp.Phone);
                    return Ok(authResult); // Flutter will see IsRegistered = false and show the Profile screen
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A system error occurred during OTP verification for {Phone}.", verifyOtp.Phone);
                return StatusCode(500, "An internal error occurred. Please try again later.");
            }
        }
        [HttpPost("auth/login-customer")]
        public async Task<IActionResult> LoginCustomer([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("LoginCustomer failed model validation.");
                return BadRequest(ModelState);
            }
            try
            {
                var loginResult = await _authService.LoginAsync(loginDto);
                if(loginResult.IsSuccess == false && loginResult.IsRegistered == false)
                {
                    _logger.LogWarning("Login failed for {Phone}: {Message}", loginDto.Phone, loginResult.Message);
                    return StatusCode(404, new {message = "new user register please"});
                }
                if (loginResult.Role != RoleEnum.Customer)
                {
                    _logger.LogWarning("Access denied for {Phone}: Not a customer account.", loginDto.Phone);
                    return StatusCode(403, new { message = "Access denied. Not a customer account." });
                }

                _logger.LogInformation("Customer {Phone} logged in successfully.", loginDto.Phone);
                return Ok(loginResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during customer login for {Phone}.", loginDto.Phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        [HttpPost("auth/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("RefreshToken failed model validation.");
                return BadRequest(ModelState);
            }
            try
            {
                var refreshResult = await _tokenService.RefreshTokenAsync(refreshTokenDto.ExpiredToken, refreshTokenDto.RefreshToken);

                if (!refreshResult.IsSuccess)
                {
                    _logger.LogWarning("Token refresh failed: {Message}", refreshResult.Message);
                    return Unauthorized(new { Message = refreshResult.Message });
                }

                _logger.LogInformation("Token refreshed successfully for UserId {UserId}.", refreshResult.UserId);
                return Ok(refreshResult);
            }
            catch (SecurityTokenException ex) // Catch specific security errors
            {
                _logger.LogInformation(ex.Message);
                // This tells the Flutter app: "Your session is fully expired. Go to Login screen."
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during token refresh.");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

        // Removed the [Authorize] attribute so users can still logout successfully 
        // even if their access token has recently expired.
        [HttpPatch("auth/logout")]
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
