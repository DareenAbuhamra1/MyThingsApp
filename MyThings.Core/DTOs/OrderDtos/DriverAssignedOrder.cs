using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class DriverAssignedOrder
{
    public int OrderId {get;set;}
    public string PartnerName {get;set;} = null!;
    public string PartnerLocation {get;set;} = null!;
    public string CustomerName {get;set;} = null!;
    public string CustomerPhone {get;set;} = null!;
    public string DeliveryLocation {get;set;} = null!;
    public bool IsReadyForPickup {get;set;}
    public OrderStatusEnum Status {get;set;}
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFee {get;set;}
    public decimal TotalPayment {get;set;}
    public TimeOnly StartEstimation {get;set;}
    public TimeOnly EndEstimation {get;set;}
    
}