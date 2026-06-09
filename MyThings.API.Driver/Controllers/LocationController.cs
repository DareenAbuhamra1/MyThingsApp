using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace LocationController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(IDriverService driverService,ILogger<LocationController> logger)
        {
            _driverService = driverService;
            _logger =logger;
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("Driver/{driverId:int}/location/{lat:decimal}/{lon:decimal}")]
        public async Task<IActionResult> UpdateLocation([FromRoute] int driverId, [FromRoute] decimal lat, [FromRoute] decimal lon)
        {
            try
            {
                var result = await _driverService.UpdateLiveLocationAsync(driverId, lat, lon);
                if(!result) return StatusCode(404, new { Message = "Driver not found"});

                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order."});
            }
        }
        
    }
}