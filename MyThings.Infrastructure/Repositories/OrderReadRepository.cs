using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class OrderReadRepository : ReadOnlyRepository<Order>, IOrderReadRepository
{
    public OrderReadRepository(ReadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(int customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Partner)
                .ThenInclude(pl => pl.Location)
            .Include(o => o.Driver)
            .Include(o => o.Domain)
            .Include(o => o.DeliveryLocation)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions)
            .ToListAsync();
    }
    public async Task<Order?> GetOrderByOrderIdAsync(int orderId)
    {
        return await _context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.Partner)
                .ThenInclude(pl => pl.Location)
            .Include(o => o.Driver)
            .Include(o => o.DeliveryLocation)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions)
            .FirstOrDefaultAsync();
    }
  
    public async Task<IReadOnlyList<Order>> GetPartnerPlacedOrdersAsync(int partnerId)
    {
        return await _context.Orders
            .Where(o => o.PartnerId == partnerId && o.Status == OrderStatusEnum.Placed)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions)
            .ToListAsync();
    }

    public async Task<Order?> GetPartnerPlacedOrderAsync(int orderId,int partnerId)
    {

        return await _context.Orders
            .Where(o => o.Id == orderId)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions)
            .FirstOrDefaultAsync();
    }

    public async Task<List<NearestDriversDto>> FindNearestDriversAsync(decimal orderLat, decimal orderLon)
    {
        return await _context.Database
            .SqlQuery<NearestDriversDto>($"EXEC dbo.GetNearestDrivers @orderLat = {orderLat}, @orderLon = {orderLon}")
            .ToListAsync(); 
    }

    public async Task<List<DriverOrderInfo>> FindNearestOrdersAsync(decimal driverLat, decimal driverLon)
    {
        return await _context.Database
            .SqlQuery<DriverOrderInfo>($"EXEC dbo.GetNearestOrders @driverLat = {driverLat}, @driverLon = {driverLon}")
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Order?>> GetPartnerPreparingOrdersAsync(int parterId)
    {
        return await _context.Orders
            .Where(o => o.PartnerId == parterId && o.Status == OrderStatusEnum.Accepted ||o.Status == OrderStatusEnum.Assigned)
            .Include(o => o.OrderLines)
                .ThenInclude(ol => ol.OrderLineOptions)
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.Driver)
            .Include(o => o.Domain)
            .Include(o => o.DeliveryLocation)
            .Include(o => o.Partner)
                .ThenInclude(pl => pl.Location)
            .ToListAsync();
    }
}