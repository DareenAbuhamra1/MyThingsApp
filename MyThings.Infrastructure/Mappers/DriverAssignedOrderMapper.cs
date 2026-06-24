using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DriverAssignedOrderMapper
{
    [MapProperty(nameof(Order.Id), nameof(DriverAssignedOrder.OrderId))]
    [MapProperty(nameof(Order.Partner.Name), nameof(DriverAssignedOrder.PartnerName))]
    [MapProperty(nameof(Order.Partner.Location), nameof(DriverAssignedOrder.PartnerLocation))]
    [MapProperty(nameof(Order.Customer), nameof(DriverAssignedOrder.CustomerName))]
    [MapProperty(nameof(Order.Customer.Phone), nameof(DriverAssignedOrder.CustomerPhone))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(DriverAssignedOrder.DeliveryLocation))]
    [MapProperty(nameof(Order.DeliveryFees), nameof(DriverAssignedOrder.DeliveryFee))]
    [MapProperty(nameof(Order.Status), nameof(DriverAssignedOrder.IsReadyForPickup))]
    public partial DriverAssignedOrder Map(Order o);

    private string MapPartnerLocation(Location location)
        => location is null
            ? string.Empty
            : $"{location.Street}, {location.Area}, {location.City.ToString()}";

    private bool MapIsReadyForPickup(OrderStatusEnum status)
        => status == OrderStatusEnum.ReadyForPickUp;
    private string MapCustomerName(Customer customer)
        => customer is null ? string.Empty :
            $"{customer.FirstName} {customer.LastName}";

}