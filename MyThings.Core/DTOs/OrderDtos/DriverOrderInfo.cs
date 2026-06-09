using MyThings.Core.Enums;
using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;

public class DriverOrderInfo
{
    public int Id {get;set;}
    public decimal SubTotal {get;set;}
    public decimal TotalPayment {get;set;}
    public decimal DeliveryFees {get;set;}
    public OrderStatusEnum Status {get;set;}
    public int PartnerId {get;set;}
    public string PartnerName {get;set;} = string.Empty;
    public CountryEnum Country {get;set;}
    public CityEnum City {get;set;}
    public string Area {get;set;} = string.Empty;
    public string Street {get;set;} = string.Empty;
    public decimal Latitude {get;set;}
    public decimal Longitude {get;set;}
    public double DistanceInKm {get;set;}
}