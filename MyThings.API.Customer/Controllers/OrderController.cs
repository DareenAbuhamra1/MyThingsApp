
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;
using OfficeOpenXml.Packaging.Ionic.Zip;

namespace OrderController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [Authorize(RoleEnum.Customer)]
        [HttpGet("Orders/{customerId:int}")]
        public async Task<IActionResult> GetAllCustomerOrders([FromRoute] int customerId)
        {
            try
            {
                var result = await _orderService.GetAllCustomerOrdersAsync(customerId);
                
                return Ok(result.Data);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetAllCustomerOrders method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        [Authorize(RoleEnum.Customer)]
        [HttpPost("add-order-cart")]
        public async Task<IActionResult> AddCart([FromBody] OrderCartDto placementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _orderService.AddOrderCartAsync(placementDto);
                if(result is null) return BadRequest("Failed to add item to cart.");
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in AddCart method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");

            }   
        }
        [Authorize(RoleEnum.Customer)]
        [HttpPost("add-item")]
        public async Task<IActionResult> AddItemToCart([FromBody] OrderLineCartDto placementDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _orderService.AddOrderLineAsync(placementDto);
                if(result is null) return BadRequest("Failed to add item to cart.");
                return Ok(result);
            }
            catch(Exception e)
            {
                 _logger.LogError(e, "An error occurred in AddItemToCart method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }
        }
        
        [Authorize(RoleEnum.Customer)]
        [HttpPatch("place-order/{orderId:int}")]
        public async Task<IActionResult> PlaceOrder([FromRoute] int orderId )
        {
            try
            {
                var result = await _orderService.PlaceOrderAsync(orderId);
                if(result is null) return BadRequest("Failed to place order.");
                return Ok(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred in PlaceOrder method");
                return StatusCode(500, "An internal server error occurred. Please try again later.");

            }   
        }
    }
}