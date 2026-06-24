using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using Riok.Mapperly.Abstractions;

namespace MyThings.Infrastructure.Mappers;

[Mapper(UseDeepCloning = false, RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class OrderCartResponseMapper
{
    [MapProperty(nameof(Order.Id), nameof(OrderCartResponseDto.OrderId))]// nullPointerReference
    [MapProperty(nameof(Order.DeliveryLocation), nameof(OrderCartResponseDto.DeliveryLocation))]
    [MapProperty(nameof(Order.Partner.Name), nameof(OrderCartResponseDto.PartnerName))]
    [MapProperty(nameof(Order.TotalPayment), nameof(OrderCartResponseDto.TotalPrice))]
    [MapProperty(nameof(Order.OrderLines), nameof(OrderCartResponseDto.OrderLine))]
    public partial OrderCartResponseDto Map(Order o);

    private string MapDeliveryLocation(Location location)
        => location is null ?
        string.Empty :
        $"{location.Street}, {location.Area}, {location.City.ToString()}";

    private OrderLineCartResponse MapOrderLine(ICollection<OrderLine> orderLines)
    {
        var line = orderLines.First();

        return new OrderLineCartResponse
        {
            ProductId = line.ProductId,
            ProductName = line.ProductName,
            Quantity = line.Quantity,
            OrderLineOptions = line.OrderLineOptions.Select(olp => new OrderLineOptionsCartResponse
            {
                ProductOptionId = olp.ProductOptionId,
                ProductOption = olp.ProductOption.Option,
                Quantity = olp.Quantity
            }).ToList()
        };
    }
}
/*

public class OrderCartResponseDto
{
    public int OrderId {get;set;}
    public required string DeliveryLocation{get;set;}
    public required OrderStatusEnum Status {get;set;}
    public required int CustomerId {get;set;}
    public required int PartnerId {get;set;}
    public required string PartnerName {get;set;}
    public required decimal SubTotal {get;set;}
    public required decimal DeliveryFees {get;set;}
    public required decimal TotalPrice {get;set;}
    public required OrderLineCartResponse OrderLine {get;set;} = null!;
}
public class OrderLineCartResponse
{
    public required int ProductId {get;set;}
    public required string ProductName {get;set;}
    public required int Quantity {get;set;}
    public List<OrderLineOptionsCartResponse>? OrderLineOptions {get;set;}
}
public class OrderLineOptionsCartResponse
{
    public int ProductOptionId {get;set;}
    public string ProductOption {get;set;} = null!;
    public int Quantity {get;set;}
}
*/