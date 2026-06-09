using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace LocationController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILogger<LocationController> _logger;
        private readonly ILocationService _locationService;



        public LocationController(ILogger<LocationController> logger, ILocationService locationService,IReadUnitOfWork readUnitOfWork)
        {
            _logger = logger;
            _locationService = locationService;
            
        }

        [Authorize(RoleEnum.Customer)]
        [HttpGet("default/{customerId}:int")]
        public async Task<IActionResult> GetDefaultLocation([FromRoute] int customerId)
        {
            try
            {
                var Location = await _locationService.GetCustomerDefaultLocation(customerId);

                if(Location is null) return StatusCode(404, " Customer location not found");

                return Ok(Location);

            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error in GetDefaultLocation for customer");
                return StatusCode(500, "Internal server error");
            }
        }
        
        [Authorize(RoleEnum.Customer)]
        [HttpPost("add")]
        public async Task<IActionResult> CreateDefaultLocation([FromBody] CustomerLocationDto locationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _locationService.CreateDefaultLocation(locationDto);
                if(result is null) return BadRequest("Failed to create location");
                return Ok(result);

            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error in SetDefaultLocation for customer");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}