using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class OrderInfoDto
{
    public required int OrderId {get;set;}
    public required OrderStatusEnum Status {get;set;}
    public string OrderStatus => Status.ToString();
    public required string PartnerName {get;set;}
    public required string Area {get;set;}
    public DateOnly PlacedDate {get;set;}
    public TimeOnly PlacedTime {get;set;}
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFees {get;set;}
    public decimal SavingAmount {get;set;}
    public required decimal TotalPrice {get;set;}
    public required string DeliveryLocation {get;set;}
    public string PaymentMethod {get;set;} = string.Empty;
    public string DriverName {get;set;} = string.Empty;
    public required List<OrderItem> OrderItems {get;set;} = [];

}
public class OrderItem
{
    public required int OrderItemId {get;set;}
    public required string OrderItemName {get;set;}
    public required decimal OrderItemPrice {get;set;}
    public required int Quantity {get;set;}
    public required List<OrderItemOption>? OrderItemOptions {get;set;} = [];
}
public class OrderItemOption
{
    public required int OrderItemOptionId {get;set;}
    public required string OrderItemOptionName {get;set;}
    public required int OrderItemOptionQuantity {get;set;}
    public required decimal OrderItemOptionPrice {get;set;}

}