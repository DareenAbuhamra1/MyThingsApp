using MyThings.Core.DTOs;
using MyThings.Core.Enums;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IOrderService
{
    Task<ServiceResponse<IReadOnlyList<AdminOrderDto>>> GetAllOrdersAsync();
    Task<ServiceResponse<IReadOnlyList<OrderInfoDto>>> GetAllCustomerOrdersAsync(int customerId);
    Task<ServiceResponse<OrderCartResponseDto>> AddOrderCartAsync(OrderCartDto orderCart);
    Task<ServiceResponse<OrderCartResponseDto>> AddOrderLineAsync(OrderLineCartDto orderLine);
    Task<ServiceResponse<OrderPlacementResponseDto>> PlaceOrderAsync(int orderId);
    Task<ServiceResponse<bool>> ReceiveOrderAsync(int orderId, int partnerId,OrderStatusEnum status);
    Task<ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>> GetPartnerPlacedOrdersAsync(int partnerId);
    Task<ServiceResponse<PartnerOrderInfoDto>> GetPartnerPlacedOrderAsync(int orderId,int partnerId);
    Task<ServiceResponse<List<DriverOrderInfo>>> GetNearestOrders(int driverId);
    Task<ServiceResponse<bool>> AssignDriverToOrder(int orderId, int driverId);
    Task<ServiceResponse<bool>> SetOrderToReadyForPickupAsync(int orderId, int partnerId);
    Task<ServiceResponse<IReadOnlyList<PartnerOrderInfoDto>>> GetPartnerPreparingOrdersAsync(int partnerId);
    Task<ServiceResponse<bool>> PickOrderAsync(int orderId, int driverId);
    Task<ServiceResponse<bool>> DeliverOrderAsync(int orderId, int driverId);
    Task<ServiceResponse<bool>> CancelOrderAsync(int orderId);

}