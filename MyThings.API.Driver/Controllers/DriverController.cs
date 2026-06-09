using System.Security;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;

namespace DriverController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly ILogger<DriverController> _logger;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public DriverController(ILogger<DriverController> logger, IAuthService authService, ITokenService tokenService)
        {
            _logger = logger;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("auth/request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] string phone)
        {
            try
            {
                int isValid = await _authService.RequestOtpAsync(phone);

                if (isValid != -1)
                {
                    _logger.LogInformation("OTP requested successfully for phone {Phone}.", phone);
                    return Ok(new { Message = "OTP sent successfully", Otp = isValid });
                }
                else
                {
                    _logger.LogWarning("Failed to request OTP for phone {Phone}.", phone);
                    return BadRequest(new { Message = "Failed to send OTP. Please try again." });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while requesting OTP for phone {Phone}.", phone);
                return StatusCode(500, new { Message = "An internal server error occurred. Please try again later." });
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
                    _logger.LogWarning("OTP verification failed for phone {Phone}: {Message}", verifyOtp.Phone, authResult.Message);
                    return BadRequest(new { Message = authResult.Message });
                }
                _logger.LogInformation("OTP verified successfully for phone {Phone}.", verifyOtp.Phone);
                return Ok(authResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while verifying OTP for phone {Phone}.", verifyOtp.Phone);
                return StatusCode(500, new { Message = "An internal server error occurred. Please try again later." });
            }
        }
        [HttpPost("auth/register-driver")]
        public async Task<IActionResult> RegisterDriver([FromBody] DriverRegisterDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var registerResult = await _authService.RegisterDriverAsync(request);
                if (!registerResult.IsSuccess)
                {
                    _logger.LogWarning("Driver registration failed for {Phone}: {Message}", request.Phone, registerResult.Message);
                    return BadRequest(new { Message = registerResult.Message });
                }
                _logger.LogInformation("Driver with {Phone} registered successfully.", request.Phone);
                return Ok(registerResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while registering a new driver with phone {Phone}.", request.Phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        [HttpPost("auth/login-driver")]
        public async Task<IActionResult> LoginDriver([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var loginResult = await _authService.LoginAsync(loginDto);
                if (!loginResult.IsSuccess)
                {
                    _logger.LogWarning("Driver login failed for {Phone}: {Message}", loginDto.Phone, loginResult.Message);
                    return BadRequest(new { Message = loginResult.Message });
                }
                _logger.LogInformation("Driver with {Phone} logged in successfully.", loginDto.Phone);
                return Ok(loginResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while logging in driver with phone {Phone}.", loginDto.Phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
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
                    refreshTokenDto.ExpiredToken,refreshTokenDto.RefreshToken);
                
                if(!refreshTokenResult.IsSuccess)
                {
                    _logger.LogWarning("Token refresh failed: {Message}", refreshTokenResult.Message);
                    return Unauthorized(new { Message = refreshTokenResult.Message });
                }
                _logger.LogInformation("Token refreshed successfully for User {UserId}.", refreshTokenResult.UserId);
                return Ok(refreshTokenResult);
            }
            catch (SecurityException ex)
            {
                _logger.LogError(ex,"Security error during token refresh.");
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during token refresh.");
                return StatusCode(500, new { Message = "An internal error occurred during token refresh." });
            }
        }
    }
}