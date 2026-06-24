using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class PartnerOrderInfoDto
{
    public required int Id {get;set;}
    public required OrderStatusEnum Status {get;set;}
    public string OrderStatus => Status.ToString();
    public decimal SubTotal {get;set;}
    public required decimal TotalPayment {get;set;}
    public required List<PartnerOrderItem> OrderLines {get;set;} = [];

}
public class PartnerOrderItem
{
    public required int Id {get;set;}
    public required string ProductName {get;set;}
    public required decimal Price {get;set;}
    public required int Quantity {get;set;}
    public required List<PartnerOrderItemOption>? OrderLineOptions {get;set;} = [];
}
public class PartnerOrderItemOption
{
    public required int Id {get;set;}
    public required string Option {get;set;}
    public required int Quantity {get;set;}
    public required decimal Price {get;set;}

}