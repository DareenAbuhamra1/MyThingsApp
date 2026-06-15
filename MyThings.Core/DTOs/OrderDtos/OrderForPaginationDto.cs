using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class OrderForPaginationDto
{
    public int OrderId {get;set;}
    public OrderStatusEnum Status {get;set;}
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFees {get;set;}
    public decimal TotalPayment {get;set;}
    public TimeOnly? StartEstimation {get;set;}
    public TimeOnly? EndEstimation {get;set;}
    public DateTime? PlacementTime {get;set;}
    public DateTime? AcceptedTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public string? Note {get;set;}
}