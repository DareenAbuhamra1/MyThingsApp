namespace MyThings.Core.DTOs;
public class OrderDetailedDto
{
    public string PartnerName {get;set;}
    public string PartnerLocation {get;set;}
    public string CustomerName {get;set;}
    public string CustomerLocation {get;set;}
    public string? DriverName {get;set;}
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFee {get;set;}
    public decimal TotalPayment {get;set;}
    public string PaymentType {get;set;}
    public DateTime? AcceptedTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? PlacementTime {get;set;}
    public required List<OrderLineDetils> OrderItems {get;set;} = [];

}
public class OrderLineDetils
{
    public required int OrderItemId {get;set;}
    public required string OrderItemName {get;set;}
    public required decimal OrderItemPrice {get;set;}
    public required int Quantity {get;set;}
    public required List<OrderOptionDetails>? OrderItemOptions {get;set;} = [];
}
public class OrderOptionDetails
{
    public required int OrderOptionId {get;set;}
    public required string OrderOptionName {get;set;}
    public required int OrderOptionQuantity {get;set;}
    public required decimal OrderOptionPrice {get;set;}

}