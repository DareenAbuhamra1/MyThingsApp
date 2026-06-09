using Microsoft.AspNetCore.Mvc;
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
        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }
        [Authorize(RoleEnum.Partner)]
        [HttpGet("get-all-orders/partner/{partnerId:int}")]
        public async Task<IActionResult> GetAllOrders([FromRoute] int partnerId)
        {
            try
            {
                var result = await _orderService.GetPartnerPreparingOrdersAsync(partnerId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An error occurred while retreiving partner :{partnerId} orders");
                return StatusCode(500, new { Message = "An internal error occurred while retreiving partner orders." });
            }

        }
        [Authorize(RoleEnum.Partner)]
        [HttpGet("receive-order/partner/{partnerId:int}")]
        public async Task<IActionResult> ReceiveOrder([FromRoute] int partnerId)
        {
            try
            {
                var result = await _orderService.GetPartnerPlacedOrdersAsync(partnerId);

                if (result == null)
                {
                    return StatusCode(500, new { Message = "The order service returned an empty response." });
                }
                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { Message = result.Message });
                }
                else
                {
                    return Ok(result.Data);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order." });
            }
        }

        [Authorize(RoleEnum.Partner)]
        [HttpPatch("{orderId:int}/partner/{partnerId:int}/accept")]
        public async Task<IActionResult> AcceptOrder([FromRoute] int orderId,[FromRoute] int partnerId)
        {
            try
            {
                var result = await _orderService.ReceiveOrderAsync(orderId, partnerId, OrderStatusEnum.Accepted);
                if (result == null)
                {
                    return StatusCode(500, new { Message = "The order service returned an empty response." });
                }
                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new { Message = result.Message});
                }
                else
                {
                    return Ok(result.Data);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order." });
            }
        }
        [Authorize(RoleEnum.Partner)]
        [HttpPatch("{orderId:int}/partner/{partnerId:int}/decline")]
        public async Task<IActionResult> DeclineOrder([FromRoute] int orderId, [FromRoute]int partnerId)
        {
            try
            {
                var result = await _orderService.ReceiveOrderAsync(orderId, partnerId, OrderStatusEnum.Cancelled);
                return Ok(new { Message = "Order declined successfully." });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An error occurred while receiving order");
                return StatusCode(500, new { Message = "An internal error occurred while receiving the order." });
            }
        }
        [Authorize(RoleEnum.Partner)]
        [HttpPatch("{orderId:int}/partner/{partnerId}/ready-for-pickup")]
        public async Task<IActionResult> SetOrderReadyForPickup([FromRoute]int orderId, [FromRoute]int partnerId)
        {
            try
            {
                var result = await _orderService.SetOrderToReadyForPickupAsync(orderId,partnerId);
                if(!result.Success) return StatusCode(result.StatusCode,new {Message = result.Message});
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred in SetOrderReadyForPickup");
                return StatusCode(500, new { Message = "An internal error occurred while setting order to ready for pickup." });
            }
            }
        }

}
