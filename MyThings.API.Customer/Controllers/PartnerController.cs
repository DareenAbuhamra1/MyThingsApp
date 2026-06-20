using Microsoft.AspNetCore.Mvc;
using Mythings.Core.Interaces.Services;
using MyThings.Core.DTOs;
using MyThings.Core.DTOs.SPSearch;
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
        private readonly ICustomerPartnerService _customerPartnerService;
        private readonly IPartnerService _partnerService;


        public PartnerController(ILogger<PartnerController> logger, ICustomerPartnerService customerPartnerService ,  IPartnerService partnerService)
        {
            _logger = logger;
            _customerPartnerService = customerPartnerService;
            _partnerService = partnerService;
        }

        [Authorize(RoleEnum.Customer)]
        [HttpGet("{domainId}")]
        public async Task<IActionResult> GetAllStoresForDomain([FromRoute] string domainId)
        {
            try
            {
                int DomainId = int.Parse(domainId);
                var Partners = await _customerPartnerService.GetPartnersAsync(DomainId);

                if (Partners == null) return NotFound($"Domain with ID {domainId} not found.");

                return Ok(Partners);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllStoresPerDomain method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        //[Authorize(RoleEnum.Customer)]
       [HttpGet("search")]
        public async Task<IActionResult> SearchOverDomain([FromQuery] SearchPartnersQueryDto dto)
        {
            try
            {
                var result = await _partnerService.SearchOverDomain(dto);
                return Ok(result);
            }catch(Exception e)
            {
                _logger.LogError(e, "Error in SearchOverDomain");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        
    }

}

