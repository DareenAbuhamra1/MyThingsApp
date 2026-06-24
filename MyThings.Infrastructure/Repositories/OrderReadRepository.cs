using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Extensions;

namespace MyThings.Infrastructure.Repositories;

public class OrderReadRepository : ReadOnlyRepository<Order>, IOrderReadRepository
{
    public OrderReadRepository(ReadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Order>> GetAllCustomerOrdersAsync(int customerId)
    {
        return await _context.Orders
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.CreatedAt)
            .Include(o => o.Driver)
            .Include(o => o.Domain)
            .Include(o => o.DeliveryLocation)
            .IncludePartnerLocation()
            .IncludeOrderLineDetails()
            .ToListAsync();
    }
    public async Task<Order?> GetOrderByOrderIdAsync(int orderId)
    {
        return await _context.Orders
            .Where(o => o.Id == orderId)
            .IncludePartnerLocation()
            .Include(o => o.Driver)
            .Include(o => o.DeliveryLocation)
            .IncludeOrderLineDetails()
            .FirstOrDefaultAsync();
    }
  
    public async Task<IReadOnlyList<Order>> GetPartnerPlacedOrdersAsync(int partnerId)
    {
        return await _context.Orders
            .Where(o => o.PartnerId == partnerId && o.Status == OrderStatusEnum.Placed)
            .IncludeOrderLineDetails()
            .ToListAsync();
    }

    public async Task<Order?> GetPartnerPlacedOrderAsync(int orderId,int partnerId)
    {

        return await _context.Orders
            .Where(o => o.Id == orderId)
            .IncludeOrderLineDetails()
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
            .Where(o => o.PartnerId == parterId && o.Status == OrderStatusEnum.Accepted || o.Status == OrderStatusEnum.Assigned)
            .IncludeOrderLineDetails()
            .ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.Driver)
            .Include(o => o.Domain)
            .IncludeCustomerLocation()
            .IncludePartnerLocation()
            .ToListAsync();
    }

    public async Task<Order?> GetCustomerPendingOrderAsync(int customerId)
    {
        return await _context.Orders
            .Where(o =>o.CustomerId == customerId && o.Status == OrderStatusEnum.Pending)
            .FirstOrDefaultAsync();     
    }

    public async Task<Order?> GetCustomerCartAsync(int customerId)
    {
        return await _context.Orders
            .Where(o =>o.CustomerId == customerId && o.Status == OrderStatusEnum.Pending)
            .Include(o => o.DeliveryLocation)
            .Include(o => o.Partner)
            .IncludeOrderLineDetails()
            .FirstOrDefaultAsync();  
    }

    public IQueryable<Order> GetPartnerOrders(int partnerId)
    {
        return _context.Orders
            .Where(o => o.PartnerId == partnerId)
            .AsQueryable();
    }

    public IQueryable<Order> GetDriverAssignedOrder(int driverId)
    {
        return  _context.Orders
            .Where(o => o.DriverId == driverId && (o.Status == OrderStatusEnum.Assigned || o.Status ==OrderStatusEnum.ReadyForPickUp ||o.Status == OrderStatusEnum.PickedUp))
            .IncludeCustomerLocation()
            .IncludePartnerLocation()
            .AsQueryable();
    }

    public IQueryable<Order> GetOrderDetails(int orderId)
    {
        return _context.Orders
            .Where(o => o.Id == orderId)
            .IncludeAllDetails()
            .AsQueryable();
    }

    public IQueryable<Order> GetAllOrders()
    {
        return _context.Orders
            .IncludeCustomerLocation()
            .IncludePartnerLocation()
            .AsQueryable();
    }
}