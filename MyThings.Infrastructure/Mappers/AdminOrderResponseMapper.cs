using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class AdminOrderResponseMapper
{
    
    [MapProperty(nameof(Order.Customer), nameof(AdminOrderResponse.CustomerFullName))]
    [MapProperty(nameof(Order.Customer.Phone), nameof(AdminOrderResponse.CustomerPhone))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(AdminOrderResponse.CustomerLocation))]
    [MapProperty(nameof(Order.Partner.Name), nameof(AdminOrderResponse.PartnerName))]
    [MapProperty(nameof(Order.Partner.Location), nameof(AdminOrderResponse.PartnerLocation))]
    [MapProperty(nameof(Order.Partner.CommissionRate), nameof(AdminOrderResponse.CommissionRate))]
    [MapProperty(nameof(Order.Driver), nameof(AdminOrderResponse.DriverFullName))]
    [MapProperty(nameof(Order.Id), nameof(AdminOrderResponse.OrderId))]
    [MapProperty(nameof(Order.DeliveryFees), nameof(AdminOrderResponse.DeliveryFee))]
    [MapProperty(nameof(Order.TotalPayment), nameof(AdminOrderResponse.Total))]
    [MapProperty(nameof(Order), nameof(AdminOrderResponse.PartnerCommissionAmount))]
    [MapProperty(nameof(Order.OrderLines), nameof(AdminOrderResponse.OrderLines))]
    [MapProperty(nameof(Order.DeliveryRuleId), nameof(AdminOrderResponse.DeliveryRuleId))]
    [MapProperty(nameof(Order.DeliveryRule.BaseFee), nameof(AdminOrderResponse.BaseFee))]
    [MapProperty(nameof(Order.DeliveryRule.PerKmFee), nameof(AdminOrderResponse.PerKmFee))]
    [MapProperty(nameof(Order.DeliveryRule.MinTotalForFreeDelivery), nameof(AdminOrderResponse.MinForFreeDelivery))]
    public partial AdminOrderResponse Map(Order o);

    private string MapCustomerFullName(Customer customer)
        => customer is null ? string.Empty : $"{customer.FirstName} {customer.LastName}";

    private string MapCustomerLocation(Location location)
        => location is null ? string.Empty : $"{location.Street}, {location.Area}, {location.City}";

    private string MapDriverFullName(Driver driver)
        => driver is null ? string.Empty : $"{driver.FirstName} {driver.LastName}";

    private decimal MapCommissionRate(decimal commissionRate)
        => commissionRate;

    private decimal MapPartnerCommissionAmount(Order order)
        => (order?.Partner?.CommissionRate ?? 0m) * (order?.SubTotal ?? 0m);
    
    [MapProperty(nameof(OrderLine.Id), nameof(AdminOrderLine.OrderLineId))]

    public partial AdminOrderLine Map(OrderLine ol);
    [MapProperty(nameof(OrderLineOption.Id), nameof(AdminOrderLineOptions.OrderLineOptionId))]

    public partial AdminOrderLineOptions Map(OrderLineOption olp);
    
}