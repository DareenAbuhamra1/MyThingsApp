using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace StatusConroller.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController: ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<StatusController> _logger;

        public StatusController(IDriverService driverService, ILogger<StatusController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("{driverId:int}/online")]
        public async Task<IActionResult> SetOnline([FromRoute] int driverId)
        {
            try
            {
                var result = await _driverService.ToggleOnlineAsync(driverId, true);
                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { Message = result.Message });
                }
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order." });
            }   
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("{driverId:int}/offline")]
        public async Task<IActionResult> SetOffline([FromRoute] int driverId)
        {
            try
            {
                var result = await _driverService.ToggleOnlineAsync(driverId, false);
                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { Message = result.Message });
                }
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order." });
            }   
        }
    }
} 