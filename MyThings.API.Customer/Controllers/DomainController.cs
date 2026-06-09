using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace DomainController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly ILogger<DomainController> _logger;
        private readonly IDomainService _domainService;


        public DomainController(ILogger<DomainController> logger,IDomainService domainService)
        {
            _logger = logger;
            _domainService = domainService;
        }

        [Authorize(RoleEnum.Customer)]
        [HttpGet("domains")]
        public async Task<IActionResult> GetAllDomains()
        {
            try
            {
                var domains = await _domainService.GetAllDomainsAsync();
                if(domains == null) return NotFound("No domains found.");

                return Ok(domains);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllProductsForStore method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        
    }

}