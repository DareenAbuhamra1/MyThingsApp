namespace MyThings.Core.DTOs;

public class AdminOrderDetails
{
    public DateTime? From {get;set;}
    public DateTime? To {get;set;}
    public string? PartnerIds {get;set;}
    public string? OrderStatuses {get;set;}
    public string? OrderIds {get;set;}
    public int PageSize {get;set;} = 10;
    public int PageNumber {get;set;} = 1;
}