using MyThings.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Core.Enums;
using MyThings.Infrastructure.Services;

namespace LogisticController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogisticController : ControllerBase
    {
        private readonly ILogisiticService _logisticService;
        private readonly ILogger<LogisticController> _logger;

        public LogisticController(ILogisiticService logisticService, ILogger<LogisticController> logger)
        {
            _logisticService = logisticService;
            _logger = logger;
        }

        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("add-delivery-rule")]
        public async Task<IActionResult> CreateDeliveryRule([FromBody] DeliveryRuleDto deliveryRule)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            { 
                var createDeliveryRuleResult = await _logisticService.CreateDeliveryRuleAsync(deliveryRule);
                if (createDeliveryRuleResult is null)
                {
                    _logger.LogWarning("Failed to create delivery rule for city {City}.", deliveryRule.City);
                    return BadRequest(new { Message = "Failed to create the delivery rule. Please try again." });
                }
                _logger.LogInformation("Delivery rule for city {City} created successfully.", deliveryRule.City);
                return Ok(createDeliveryRuleResult);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while creating the delivery rule.");
                return StatusCode(500, new { Message = "An internal error occurred while creating the delivery rule." });
            }                                  
        }
    }
}