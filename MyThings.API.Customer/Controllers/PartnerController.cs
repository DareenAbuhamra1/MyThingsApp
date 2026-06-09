using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace PartnerController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly ILogger <PartnerController> _logger;
        private readonly ICustomerPartnerService _partnerService;


        public PartnerController(ILogger<PartnerController> logger, ICustomerPartnerService partnerService)
        {
            _logger = logger;
            _partnerService = partnerService;
        }

        [Authorize(RoleEnum.Customer)]
        [HttpGet("{domainId}")]
        public async Task<IActionResult> GetAllStoresForDomain([FromRoute] string domainId)
        {
            try
            {
                int DomainId = int.Parse(domainId);
                var Partners = await _partnerService.GetPartnersAsync(DomainId);

                if (Partners == null) return NotFound($"Domain with ID {domainId} not found.");

                return Ok(Partners);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllStoresPerDomain method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        
    }

}

