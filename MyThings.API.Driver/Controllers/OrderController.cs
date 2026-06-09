using Microsoft.AspNetCore.Mvc;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;

namespace OrderController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(ILogger<OrderController> logger,IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }
        
        [Authorize(RoleEnum.Driver)]
        [HttpGet("receive-order/driver/{driverId:int}")]
        public async Task<IActionResult> ReceiveNearOrders([FromRoute] int driverId)
        {
            try
            {
                var result = await _orderService.GetNearestOrders(driverId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message });

                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order."});
            }
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("{orderId:int}/accept/{driverId:int}")]
        public async Task<IActionResult> AcceptOrder([FromRoute]int orderId,[FromRoute] int driverId)
        {
            try
            {
                var result = await _orderService.AssignDriverToOrder(orderId,driverId);
                if(!result.Success) return StatusCode(result.StatusCode, new {Message = result.Message});
                
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while driver {driverId} accepted order {orderId}");
                return StatusCode(500, new { Message = "An internal error occurred while driver {driverId} accepted order {orderId}"});
            }
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("{orderId:int}/pickup/driver/{driverId:int}")]
        public async Task<IActionResult> PickupOrder([FromRoute] int orderId, [FromRoute] int driverId)
        {
            try
            {
                var result = await _orderService.PickOrderAsync(orderId,driverId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An error occurred while driver {driverId} picked order {orderId} in PickOrder in OrderController ");
                return StatusCode(500, new { Message = "An internal error occurred while driver {driverId} picked order {orderId} in PickOrder in OrderController "});
            }
        }
        [Authorize(RoleEnum.Driver)]
        [HttpPatch("{orderId:int}/deliver/driver/{driverId:int}")]
        public async Task<IActionResult> DeliverOrder([FromRoute] int orderId, [FromRoute] int driverId)
        {
            try
            {
                var result = await _orderService.DeliverOrderAsync(orderId,driverId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"An error occurred while driver {driverId} delivered order {orderId} in DeliverOrder in OrderController ");
                return StatusCode(500, new { Message = "An internal error occurred while driver {driverId} delivered order {orderId} in DeliverOrder in OrderController "});
            }
        }
    }
}