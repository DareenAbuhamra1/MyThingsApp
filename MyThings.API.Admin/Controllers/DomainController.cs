using MyThings.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Core.Enums;
using MyThings.Infrastructure.Services;

namespace DomainController.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly ILogger<DomainController> _logger;
        private readonly IDomainService _domainService;

        public DomainController(ILogger<DomainController> logger, IDomainService domainService)
        {
            _logger = logger;
            _domainService = domainService;
        }

        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("create-domain")]
        public async Task<IActionResult> CreateDomain([FromBody] DomainCreationDto domainCreation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createDomainResult = await _domainService.CreateDomainAsync(domainCreation);
                if (createDomainResult is null)
                {
                    _logger.LogWarning("Failed to create domain with name {DomainName}.", domainCreation.Name);
                    return BadRequest(new { Message = "Failed to create the domain. Please try again." });
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating domain with name {DomainName}.", domainCreation.Name);
                return StatusCode(500, new { Message = "An internal error occurred while creating the domain." });
            }
            return Ok(new { Message = "Domain created successfully." });
        }
        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("attach-partner-domain")]
        public async Task<IActionResult> AttachPartnerDomain([FromBody] PartnerDomainDto partnerDomain)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var createDomainResult = await _domainService.AttachPartnerToDomainAsync(partnerDomain.PartnerId, partnerDomain.DomainId);
                if (createDomainResult is false)
                {
                    _logger.LogWarning("Failed to attach partner with id {PartnerId} to domain with id {DomainId}.", partnerDomain.PartnerId, partnerDomain.DomainId);
                    return BadRequest(new { Message = "Failed to attach the partner to the domain. Please try again." });
                }
                _logger.LogInformation("Partner with id {PartnerId} attached to domain with id {DomainId} successfully.", partnerDomain.PartnerId, partnerDomain.DomainId);
                return Ok(new { Message = "Partner attached to domain successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while creating partner-domain.");
                return StatusCode(500, new { Message = "An internal error occurred while creating the partner-domain." });
            }
        }
    }
}