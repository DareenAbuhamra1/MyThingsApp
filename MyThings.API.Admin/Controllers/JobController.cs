using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace JobController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ILogger<JobController> _logger;
        public JobController(IJobService jobService, ILogger<JobController> logger)
        {
            _jobService = jobService;
            _logger = logger;
        }

        [Authorize(RoleEnum.SuperAdmin)]
        [HttpPost("add-job")]
        public async Task<IActionResult> CreateJob(JobDto job)
        {
            try
            {
                var result = await _jobService.CreateJob(job);

                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Internal server error in creating job");
                return StatusCode(500, new { Message = "Internal server error in creating job"});
            }
        }
    }
}