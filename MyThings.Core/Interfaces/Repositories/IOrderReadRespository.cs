using MyThings.Core.DTOs;
using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;


public interface IOrderReadRepository
{
    Task<IEnumerable<Order>> GetAllOrdersAsync();
    Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(int customerId);
    Task<Order?> GetOrderByOrderIdAsync(int orderId);
    Task<IReadOnlyList<Order>> GetPartnerPlacedOrdersAsync(int partnerId);
    Task<Order?> GetPartnerPlacedOrderAsync(int orderId,int partnerId);
    Task<List<NearestDriversDto>> FindNearestDriversAsync(decimal orderLat, decimal orderLon);
    Task<List<DriverOrderInfo>> FindNearestOrdersAsync(decimal driverLat, decimal driverLon);
    Task<IReadOnlyList<Order?>> GetPartnerPreparingOrdersAsync(int parterId);
}

