namespace MyThings.Core.DTOs;
public class OrderAdminQueryDto
{
    public int Page {get;set;} = 1;
    public int PageSize {get;set;} = 9;
    public string? Status {get;set;}
    public DateTime? From {get;set;}
    public DateTime? To {get;set;}
    public string? Search {get;set;}

}