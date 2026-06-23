using System.ServiceModel.Channels;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Helper;
using OfficeOpenXml;

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
        [Authorize(RoleEnum.Admin,RoleEnum.SuperAdmin)]
        [HttpGet("history")]
        public async Task<IActionResult> GetOrders([FromQuery] OrderAdminQueryDto queryDto)
        {
            try
            {
                var result = await _orderService.GetOrderHistoryAdminAsync(queryDto);
                
                return Ok(result.Data);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred in SetOrderReadyForPickup");
                return StatusCode(500, new { Message = "An internal error occurred while setting order to ready for pickup." });
            }
        }
        [HttpGet("get-orders-with-details")]
        public async Task<IActionResult> GetOrdersWithDetails([FromQuery] AdminOrderDetails query)
        {
            try
            {
                var response = await _orderService.GetOrdersWithDetailsAsync(query);

                return response.Success
                ? Ok(response.Data)
                : BadRequest(response.Message);
                
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred in GetOrdersWithDetails");
                return StatusCode(500, new { Message = "An internal error occurred while setting order to ready for pickup." });
            } 
        }
        [HttpGet("orders/excel")]
        public async Task<IActionResult> GetOrdersExcel()
        {
            try
            {
                var result = await _orderService.GetOrdersForExcelAsync();

                if (!result.Success)
                {
                    return StatusCode(result.StatusCode, new {Message = result.Message});
                }          
                ExcelPackage.License.SetNonCommercialPersonal("DareenAbuhamra");

                using var package = new ExcelPackage();

                var worksheet = package.Workbook.Worksheets.Add("Orders");

                worksheet.Cells["A1"].LoadFromDataTable(result.Data,true);

                var bytes = package.GetAsByteArray();

                return File(
                    bytes,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "Orders.xlsx"
                );
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error while downloading orders excel");
                return StatusCode(500, new {Message = "An interal error in downloading orders excel"});
            }
        }
    }
}