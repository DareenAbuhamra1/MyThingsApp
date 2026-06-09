using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class PartnerOrderInfoDto
{
    public required int OrderId {get;set;}
    public required OrderStatusEnum Status {get;set;}
    public string OrderStatus => Status.ToString();
    public decimal SubTotal {get;set;}
    public required decimal TotalPrice {get;set;}
    public required List<PartnerOrderItem> OrderItems {get;set;} = [];

}
public class PartnerOrderItem
{
    public required int OrderItemId {get;set;}
    public required string OrderItemName {get;set;}
    public required decimal OrderItemPrice {get;set;}
    public required int Quantity {get;set;}
    public required List<PartnerOrderItemOption>? OrderItemOptions {get;set;} = [];
}
public class PartnerOrderItemOption
{
    public required int ItemOptionId {get;set;}
    public required string ItemOptionName {get;set;}
public required int ItemOptionQuantity {get;set;}
    public required decimal ItemOptionPrice {get;set;}

}