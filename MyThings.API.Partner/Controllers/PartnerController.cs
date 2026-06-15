using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;

namespace PartnerController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PartnerController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly ILogger<PartnerController> _logger;
        public PartnerController(IAuthService authService,ITokenService tokenService,ILogger<PartnerController> logger)
        {
            _authService = authService;
            _tokenService = tokenService;
            _logger = logger;
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
                    return StatusCode(400, new { message = "Verification failed" });
                }
                if (authResult.IsRegistered)
                {
                    if (authResult.Role != RoleEnum.Partner)
                    {
                        _logger.LogWarning("Access denied for {Phone}: Not a partner account.", verifyOtp.Phone);
                        return StatusCode(403, new { message = "Access denied. Not a partner account." });
                    }

                    _logger.LogInformation("Partner {Phone} verified correctly.", verifyOtp.Phone);
                    return Ok(authResult);
                }
                else
                {
                    _logger.LogInformation("New user {Phone} verified; moving to registration.", verifyOtp.Phone);
                    return Ok(authResult); 
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "A system error occurred during OTP verification for {Phone}.", verifyOtp.Phone);
                return StatusCode(500, "An internal error occurred. Please try again later.");
            }
        }

        [HttpPost("auth/login-partner")]
        public async Task<IActionResult> LoginPartner([FromBody] LoginDto loginDto){
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var loginResult = await _authService.LoginAsync(loginDto);
                if(loginResult.IsSuccess == false)
                {
                    _logger.LogWarning("Login failed for {Phone}: {Message}", loginDto.Phone, loginResult.Message);
                    return Unauthorized(new { Message = loginResult.Message });
                }
                _logger.LogInformation("Partner {Phone} logged in successfully.", loginDto.Phone);
                return Ok(loginResult);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred during partner login for {Phone}.", loginDto.Phone);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }

    }
}