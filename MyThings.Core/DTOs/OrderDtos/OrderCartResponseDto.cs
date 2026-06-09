using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

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