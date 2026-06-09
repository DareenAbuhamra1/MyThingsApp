using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class AdminOrderDto
{
    public int OrderId {get;set;}
    public string CustomerName {get;set;} = string.Empty;
    public string PartnerName {get;set;} = string.Empty;
    public string Domain {get;set;} = string.Empty;
    public string? DriverName {get;set;} = string.Empty;
    public string DeliveryLocation {get;set;} = string.Empty;
    public string PartnerLocation {get;set;} = string.Empty;
    public string Status {get;set;} = string.Empty;
    public decimal SubTotal {get;set;} 
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFees {get;set;}
    public decimal TotalPayment {get;set;}
    public OrderPaymentTypeEnum? PaymentType {get;set;}
    public TimeOnly? StartEstimation {get;set;}
    public TimeOnly? EndEstimation {get;set;}
    public DateTime? AcceptedTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? PlacementTime {get;set;}

    
}