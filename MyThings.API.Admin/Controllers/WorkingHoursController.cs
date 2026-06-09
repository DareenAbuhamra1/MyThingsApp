using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;
using MyThings.Infrastructure.Services;

namespace WorkingHoursController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingHoursController : ControllerBase
    {
        private readonly ILogger<WorkingHoursController> _logger;
        private readonly IWorkingHoursService _workingHoursService;
        public WorkingHoursController(ILogger<WorkingHoursController> logger,IWorkingHoursService workingHoursService)
        {
            _logger = logger;
            _workingHoursService = workingHoursService;
        }
        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("add-working-hours")]
        public async Task<IActionResult> AddWorkingHours([FromBody] WorkingHoursDto dto)
        {
            try
            {
               var result = await _workingHoursService.AddWorkingHoursAsync(dto);
               if(!result.Success) return StatusCode(result.StatusCode, new {Message = result.Message});
               return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Internal server error while adding working hours");
                return StatusCode(500, "Internal server error while adding working hours");
            }
        }
    }
}