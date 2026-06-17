using Microsoft.AspNetCore.Mvc;
using Mythings.Core.Interaces.Services;
using MyThings.Core.Enums;
using MyThings.Infrastructure.Helper;

namespace AuditController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartnerController : ControllerBase
    {
        private readonly IPartnerService _partnerService;
        private readonly ILogger<PartnerController> _logger;
        public PartnerController(IPartnerService partnerService,ILogger<PartnerController> logger)
        { 
            _partnerService = partnerService;
            _logger = logger;
        }
        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [HttpGet("get-partners")]
        public async Task<IActionResult> GetAllPartners()
        {
            try
            {
                var result = await _partnerService.GetPartnerListAsync();
                return Ok(result);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error in getting partner in partner controller");
                return StatusCode(500, new { Message = "An internal error occurred" });   
            }
        }
    }
}