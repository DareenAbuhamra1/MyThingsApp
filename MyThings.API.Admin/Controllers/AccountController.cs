using MyThings.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Core.Enums;
using Grpc.Core;

namespace AccountController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger <AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPatch("activate-driver/{driverId}")]
        public async Task<IActionResult> ActivateDriver([FromRoute] int driverId)
        {
            try
            {
                var result  = await _accountService.ApproveDriverAsync(driverId, true);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occured while approving driver with ID {DriverId}.", driverId);
                return StatusCode(500, new { Message = "An internal error occurred while approving the driver." });
            }
        }
        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPatch("deactivate-driver/{driverId}")]
        public async Task<IActionResult> DeactivateDriver([FromRoute] int driverId)
        {
            try
            {
               var result  = await _accountService.ApproveDriverAsync(driverId, false);
               if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
               return Ok(result.Data);
               
            }catch(Exception e)
            {
                _logger.LogError(e, "An error occured while approving driver with ID {DriverId}.", driverId);
                return StatusCode(500, new { Message = "An internal error occurred while approving the driver." });
            }
        }
        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("create-partner")]
        public async Task<IActionResult> CreatePartner([FromBody] PartnerRegisterDto partnerRegister)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createPartnerResult = await _accountService.CreatePartnerAsync(partnerRegister);

                if (createPartnerResult == null)
                {
                    return BadRequest(new { Message = "Failed to create the partner. Please try again." });
                }
                return Ok(new { Message = "Partner created successfully.", Partner = createPartnerResult });
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error occurred while creating partner with name {partnerName}.",partnerRegister.Name);
                return StatusCode(500, new {Message = "An internal error occurred while creating the partner."});
            }
        }
        [Authorize(RoleEnum.SuperAdmin)]
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] AdminInfoDto admin)
        {
            try
            {
                var result = await _accountService.CreateAdminAsync(admin);
                if(!result.Success) return StatusCode(result.StatusCode, new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error occurred while creating admin account");
                return StatusCode(500, new {Message = "An internal error occurred while creating the admin account."});
            }
        }
    }
}