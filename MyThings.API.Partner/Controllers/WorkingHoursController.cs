using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;


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
        [Authorize(RoleEnum.Partner)]
        [HttpGet("get-working-hours/{partnerId:int}")]
        public async Task<IActionResult> GetWorkingHours([FromRoute]int PartnerId)
        {
            try
            {
                var result = await _workingHoursService.GetPartnerWorkingHoursAsync(PartnerId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});

                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception in Get Partner Working Hours");
                return StatusCode(500, new { Message = "An internal error occurred while returning partner working hours"});
            }
        }
        [Authorize(RoleEnum.Partner)]
        [HttpPatch("Partner/{partnerId:int}/close")]
        public async Task<IActionResult> ToggleManualClose([FromRoute ]int partnerId)
        {
            try
            {
                var result = await _workingHoursService.ToggleManualCloseAsync(partnerId, true);
                if(!result.Success) return StatusCode(result.StatusCode,new{Message = result.Data});

                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception toggling IsAvailable for partner {partnerId}",partnerId);
                return StatusCode(500, new { Message = "An internal error occurred while toggling IsAvailable for partner {partnerId}",partnerId});
            }
            }
        
        [Authorize(RoleEnum.Partner)]
        [HttpPatch("Partner/{partnerId:int}/open")]
        public async Task<IActionResult> ToggleManualOpen([FromRoute ]int partnerId)
        {
            try
            {
                var result = await _workingHoursService.ToggleManualCloseAsync(partnerId, false);
                if(!result.Success) return StatusCode(result.StatusCode,new{Message = result.Data});

                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Exception toggling IsAvailable for partner {partnerId}",partnerId);
                return StatusCode(500, new { Message = "An internal error occurred while toggling IsAvailable for partner {partnerId}",partnerId});
            }
        }
    }
}