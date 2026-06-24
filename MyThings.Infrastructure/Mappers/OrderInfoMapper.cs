using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target, UseDeepCloning = false)]
public partial class OrderInfoMapper
{
    [MapProperty(nameof(Order.Id), nameof(OrderInfoDto.OrderId))]
    [MapProperty(nameof(Order.Partner), nameof(OrderInfoDto.Area))]
    [MapProperty(nameof(Order.Driver), nameof(OrderInfoDto.DriverName))]
    [MapProperty(nameof(Order.TotalPayment), nameof(OrderInfoDto.TotalPrice))]
    [MapProperty(nameof(Order.OrderLines), nameof(OrderInfoDto.OrderItems))]
    [MapProperty(nameof(Order.OrderLines), nameof(OrderInfoDto.OrderItems))]
    [MapProperty(nameof(Order.PaymentType), nameof(OrderInfoDto.PaymentMethod))]
    [MapProperty(nameof(Order.PlacementTime), nameof(OrderInfoDto.PlacedDate))]
    [MapProperty(nameof(Order.PlacementTime), nameof(OrderInfoDto.PlacedTime))]
    public partial OrderInfoDto Map(Order o);
    private string MapDriverName(Driver driver)
        => driver is null
        ? string.Empty
        : $"{driver.FirstName} {driver.LastName}";
    private string MapDeliveryLocation(Location location)
        => location is null
        ? string.Empty
        : $"{location.Street}, {location.Area}, {location.City}";
    private string MapArea(Partner partner)
        => partner?.Location?.Area ?? "";

    // DateTime → DateOnly
    private DateOnly? MapPlacedDate(DateTime? placementTime)
        => placementTime is null ? null : DateOnly.FromDateTime(placementTime.Value);

    // DateTime → TimeOnly
    private TimeOnly? MapPlacedTime(DateTime? placementTime)
        => placementTime is null ? null : TimeOnly.FromDateTime(placementTime.Value);
    private string MapDriverName(Order o)
    {
        return o.Driver == null
            ? string.Empty
            : $"{o.Driver.FirstName} {o.Driver.LastName}";
    }

    [MapProperty(nameof(OrderLine.Id), nameof(OrderItem.OrderItemId))]
    [MapProperty(nameof(OrderLine.ProductName), nameof(OrderItem.OrderItemName))]
    [MapProperty(nameof(OrderLine.Price), nameof(OrderItem.OrderItemPrice))]
    [MapProperty(nameof(OrderLine.OrderLineOptions), nameof(OrderItem.OrderItemOptions))]
    public partial OrderItem Map(OrderLine ol);
    [MapProperty(nameof(OrderLineOption.Id), nameof(OrderItemOption.OrderItemOptionId))]
    [MapProperty(nameof(OrderLineOption.Option), nameof(OrderItemOption.OrderItemOptionName))]
    [MapProperty(nameof(OrderLineOption.Price), nameof(OrderItemOption.OrderItemOptionPrice))]
    [MapProperty(nameof(OrderLineOption.Quantity), nameof(OrderItemOption.OrderItemOptionQuantity))]
    public partial OrderItemOption Map(OrderLineOption olp);

}