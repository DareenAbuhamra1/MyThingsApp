using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, UseDeepCloning = false)]
public partial class AdminOrderMapper
{
    [MapProperty(nameof(Order.Id), nameof(AdminOrderDto.OrderId))]
    [MapProperty(nameof(Order.Customer), nameof(AdminOrderDto.CustomerName))]
    [MapProperty(nameof(Order.Partner.Name), nameof(AdminOrderDto.PartnerName))]
    [MapProperty(nameof(Order.Driver), nameof(AdminOrderDto.DriverName))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(AdminOrderDto.DeliveryLocation))]
    [MapProperty(nameof(Order.Status), nameof(AdminOrderDto.Status))]
    public partial AdminOrderDto Map(Order o);

    private string MapDriverName(Driver driver)
        => driver is null
        ? string.Empty
        : $"{driver.FirstName} {driver.LastName}";
    private string MapDeliveryLocation(Location location)
        => location is null
        ? string.Empty
        : $"{location.Street}, {location.Area}, {location.City}";

    private string MapCustomerName(Customer customer)
            => customer is null
            ? string.Empty
            : $"{customer.FirstName} {customer.LastName}";

}
