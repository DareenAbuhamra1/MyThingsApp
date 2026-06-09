using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
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

        [HttpPost("auth/login-partner")]
        public async Task<IActionResult> LoginPartner([FromBody] LoginDto loginDto)
        {
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