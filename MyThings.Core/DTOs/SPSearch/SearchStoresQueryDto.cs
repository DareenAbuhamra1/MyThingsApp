namespace MyThings.Core.DTOs.SPSearch;

public class SearchPartnersQueryDto
{
    public string SearchTerm {get;set;} = null!;
    public int DomainId {get;set;}
    public int PageNumber {get;set;} = 1;
    public int PageSize {get;set;} = 10;
    public decimal Latitude {get;set;}
    public decimal Longitude {get;set;}
}
