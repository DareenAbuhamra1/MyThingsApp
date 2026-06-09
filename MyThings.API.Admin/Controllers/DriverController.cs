using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace DriverController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase{

        private readonly IDriverService _driverService;
        private readonly ILogger<DriverController> _logger;
        public DriverController(IDriverService driverService,ILogger<DriverController> logger)
        {
            _driverService = driverService;
            _logger = logger;
        }
        [Authorize(RoleEnum.SuperAdmin,RoleEnum.Admin)]
        [HttpGet("get-drivers")]
        public async Task<IActionResult> GetAllDrivers()
        {
            try
            {
                var result = await _driverService.GetAllDriversAsync();
                if(!result.Success) return StatusCode(result.StatusCode, new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message,"Internal Server error in GetAllDrivers in Admin DriverController");
                return StatusCode(500, new {Message = "Internal Server error in GetAllDrivers in Admin DriverController"});
            }
        }
    }
}