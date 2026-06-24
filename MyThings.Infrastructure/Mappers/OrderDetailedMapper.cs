namespace MyThings.Infrastructure.Mappers;

using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderDetailedMapper
{

    [MapProperty(nameof(Order.OrderLines), nameof(OrderDetailedDto.OrderItems))]
    [MapProperty(nameof(Order.Partner.Name), nameof(OrderDetailedDto.PartnerName))]
    [MapProperty(nameof(Order.Customer), nameof(OrderDetailedDto.CustomerName))]
    [MapProperty(nameof(Order.DeliveryLocation), nameof(OrderDetailedDto.CustomerLocation))]
    [MapProperty(nameof(Order.Driver), nameof(OrderDetailedDto.DriverName))]
    [MapProperty(nameof(Order.DeliveryFees), nameof(OrderDetailedDto.DeliveryFee))]
    public partial OrderDetailedDto Map(Order o);
    /*
    private string MapPartnerName(Partner partner)
        => partner is null
        ? string.Empty
        : $"{partner.Name}";
        */
    private string MapCustomerName(Customer customer)
        => customer is null
        ? string.Empty
        : $"{customer.FirstName} {customer.LastName}";
    private string MapCustomerLocation(Location location)
        => location is null
        ? string.Empty
        : $"{location.Street}, {location.Area}, {location.City.ToString()}";   
    private string MapDriverName(Driver driver)
        => driver is null
        ? string.Empty
        : $"{driver.FirstName} {driver.LastName}";   
    
    [MapProperty(nameof(OrderLine.OrderId), nameof(OrderLineDetails.OrderItemId))]
    [MapProperty(nameof(OrderLine.Price), nameof(OrderLineDetails.OrderItemPrice))]
    [MapProperty(nameof(OrderLine.Quantity), nameof(OrderLineDetails.Quantity))]
    [MapProperty(nameof(OrderLine.ProductName), nameof(OrderLineDetails.OrderItemName))]
    [MapProperty(nameof(OrderLine.OrderLineOptions), nameof(OrderLineDetails.OrderItemOptions))]
    public partial OrderLineDetails Map(OrderLine ol);

    [MapProperty(nameof(OrderLineOption.ProductOptionId), nameof(OrderOptionDetails.OrderOptionId))]
    [MapProperty(nameof(OrderLineOption.Quantity), nameof(OrderOptionDetails.OrderOptionQuantity))]
    [MapProperty(nameof(OrderLineOption.Price), nameof(OrderOptionDetails.OrderOptionPrice))]
    [MapProperty(nameof(OrderLineOption.Option), nameof(OrderOptionDetails.OrderOptionName))]    
    public partial OrderOptionDetails Map(OrderLineOption olp);

}