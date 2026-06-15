using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class OrderHistoryQueryDto
{
    public int Page {get;set;} = 1;
    public int PageSize {get;set;} = 10;
    public OrderStatusEnum? Status {get;set;}
    public DateTime? From {get;set;}
    public DateTime? To {get;set;}
    public string? Search {get;set;}

}