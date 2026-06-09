using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace ProductController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductController : ControllerBase{
        private readonly ILogger<ProductController> _logger;
        private readonly ICustomerPartnerService _partnerService; //change to Product Service
        public ProductController(ILogger <ProductController> logger,ICustomerPartnerService partnerService)
        {
            _logger = logger;
            _partnerService = partnerService;
        }
        
        [Authorize(RoleEnum.Customer)]
        [HttpGet("partner/{partnerId:int}/products")]
        public async Task<IActionResult> GetAllProductsForStore([FromRoute] int partnerId)
        {
            try
            {
                var products = await _partnerService.GetProductsAsync(partnerId);

                if(products == null) return NotFound($"Store with ID {partnerId} not found.");

                return Ok(products);
                
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllProductsForStore method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");   
            }      
        }
    
        [Authorize(RoleEnum.Customer)]
        [HttpGet("{productId}/productOptions")]
        public async Task<IActionResult> GetProductOptionsForProduct([FromRoute] string productId)
        {
            try
            {
                int ProductId = int.Parse(productId);
                
                var productOptions = await _partnerService.GetProductOptionsAsync(ProductId);

                if(productOptions == null || productOptions.Count == 0) return Ok(new List<ProductOptionDisplayDto>());

                return Ok(productOptions);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetProductOptionsForProduct method");
                return StatusCode(500 , "An internal server error occurred. Please try again later."); 
            }         
        }
    }
    //selectedProduct = await Http.GetFromJsonAsync<ProductDetailsDto>($"{ApiBaseUrl}/Product/{productId}/productOptions");
}