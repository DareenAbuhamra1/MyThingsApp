using MyThings.Infrastructure.Helper;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Core.Enums;
using MyThings.Infrastructure.Services;
using StackExchange.Redis;
using MyThings.Core.Interfaces.Services;
using MyThings.Core.DTOs.CustomerAdminDtos;

namespace CustomerController.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerAdminService _customerAdminService;
        private readonly HttpClient _client;

        public CustomerController(ILogger<CustomerController> logger, ICustomerAdminService customerAdminService, IHttpClientFactory factory)
        {
            _logger = logger;
            _customerAdminService = customerAdminService;
            _client = factory.CreateClient("customer-api");
        }

        [HttpGet("customers/details")]
        public async Task<ActionResult> GetCustomersDetailsForAdmin([FromQuery] CustomerAdminFilterDto filter)
        {
            var response = await _customerAdminService.GetCustomersDetailsForAdminAsync(filter);
            return response.Success
                ? Ok(response.Data)
                : BadRequest(response.Message);
        }
        [HttpGet("customer/cacheTestEndpoint")]
        public async Task<ActionResult> CacheTestEndpoint()
        {
            _logger.LogInformation($"CacheTestEndpoint base address: {_client.BaseAddress}");
            var response = await _client.GetStringAsync("api/Cache/test-api-caching");
            
            return Ok(response);
        }
    }
}