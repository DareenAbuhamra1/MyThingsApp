namespace MyThings.Core.DTOs;
public class OrderDetailedDto
{
    public string PartnerName {get;set;} = string.Empty;
    public string PartnerLocation {get;set;} = string.Empty;
    public string CustomerName {get;set;} = string.Empty;
    public string CustomerLocation {get;set;} = string.Empty;
    public string? DriverName {get;set;}
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFee {get;set;}
    public decimal TotalPayment {get;set;}
    public string PaymentType {get;set;} = string.Empty;
    public DateTime? AcceptedTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? PlacementTime {get;set;}
    public required List<OrderLineDetails> OrderItems {get;set;} = [];

}
public class OrderLineDetails
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