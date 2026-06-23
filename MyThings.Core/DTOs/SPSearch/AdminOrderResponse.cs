using Microsoft.Identity.Client;

namespace MyThings.Core.DTOs;

public class AdminOrderResponse
{
    public int CustomerId {get;set;}
    public string CustomerFullName {get;set;} = string.Empty;
    public string CustomerPhone {get;set;} = string.Empty;
    public string CustomerLocation {get;set;} = string.Empty;
    public int PartnerId {get;set;}
    public string PartnerName {get;set;} = string.Empty;
    public decimal CommissionRate {get;set;}
    public string PartnerLocation {get;set;} = string.Empty;
    public int? DriverId {get;set;} 
    public string? DriverFullName {get;set;} = string.Empty;
    public int OrderId {get;set;}
    public int DomainId {get;set;}
    public string Status {get;set;} = string.Empty;
    public string Note {get;set;} = string.Empty;
    public DateTime? PlacementTime {get;set;}
    public DateTime? AcceptedTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public decimal SubTotal {get;set;}
    public decimal DeliveryFee {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal Total {get;set;}
    public decimal PartnerCommissionAmount {get;set;}
    public int DeliveryRuleId {get;set;}
    public decimal BaseFee {get;set;}
    public decimal PerKmFee {get;set;}
    public decimal MinForFreeDelivery {get;set;}
    public List<AdminOrderLine> OrderLines {get;set;} = [];
}
public class AdminOrderLine
{
    public int OrderLineId {get;set;}
    public int ProductId {get;set;}
    public string ProductName {get;set;} = string.Empty;
    public decimal Price {get;set;}
    public int Quantity {get;set;}
    public List<AdminOrderLineOptions> OrderLineOptions {get;set;}= [];
}
public class AdminOrderLineOptions
{
    public int OrderLineOptionId {get;set;}
    public int ProductOptionId {get;set;}
    public string Option {get;set;} = string.Empty;
    public decimal Price {get;set;}
    public int Quantity {get;set;}

}