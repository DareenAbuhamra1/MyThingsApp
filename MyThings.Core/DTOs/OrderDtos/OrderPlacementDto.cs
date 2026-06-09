using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class OrderPlacementDto
{
    public required int DeliveryLocationId {get;set;}
    public required OrderStatusEnum Status {get;set;}
    public required int CustomerId {get;set;}
    public required decimal SubTotal {get;set;}
    public required decimal ServiceFee {get;set;}
    public required decimal DeliveryFees {get;set;}
    public required decimal SavingAmount {get;set;}
    public required int DeliveryRuleId {get;set;}
    public required decimal TotalPayment {get;set;}
    public required string PaymentType {get;set;}
    public required TimeOnly StartEstimation {get;set;}
    public required TimeOnly EndEstimation {get;set;}
    public required int PartnerId {get;set;}
    public required int DomainId {get;set;}
    public string Note {get;set;} = string.Empty;
    public List<OrderLineDto> OrderLines {get;set;} = [];

}
public class OrderLineDto
{
    public required int OrderId {get;set;}
    public required int ProductId {get;set;}
    public required string ProductName {get;set;}
    public required decimal Price {get;set;}
    public required int Quantity {get;set;}
    public string Note {get;set;} = string.Empty;
    public List<OrderLineOptionDto> OrderLineOptions {get;set;} = [];

}
public class OrderLineOptionDto
{
    public required int OrderLineId {get;set;}
    public required int ProductOptionId {get;set;}
    public required string Option {get;set;}
    public required decimal Price {get;set;}
    public required int Quantity {get;set;}
}