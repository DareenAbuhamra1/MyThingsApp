using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace OrderController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger,IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }
        
        [Authorize(RoleEnum.Admin,RoleEnum.SuperAdmin)]
        [HttpGet("get-all-orders")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var result = await _orderService.GetAllOrdersAsync();
                
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error while getting all order in admin controller");
                return StatusCode(500, "An error while getting all order in admin controller");
            }
        }
        
        [Authorize(RoleEnum.Admin,RoleEnum.SuperAdmin)]
        [HttpPatch("{orderId:int}/cancel")]
        public async Task<IActionResult> CancelOrder([FromRoute] int orderId)
        {
            try
            {
                var result = await _orderService.CancelOrderAsync(orderId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error in Canceling Order");
                return StatusCode(500,"An error in Canceling Order");
            }
        }
        [Authorize(RoleEnum.Admin,RoleEnum.SuperAdmin)]
        [HttpGet("order-details/{orderId:int}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute]  int orderId){
            try
            {
                var result = await _orderService.GetOrderDetailsAsync(orderId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error in Getting Order Details");
                return StatusCode(500,"An error in Getting Order Details");
            }
        }
    }
}