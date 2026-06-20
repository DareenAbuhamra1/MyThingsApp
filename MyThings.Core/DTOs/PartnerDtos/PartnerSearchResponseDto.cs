namespace MyThings.Core.DTOs;

public class PartnerSearchResponseDto
{
    public int Id {get;set;}
    public string Name {get;set;}
    public string Description {get;set;}
    public decimal Distance {get;set;}
    public int Status {get;set;}
    public int ParentStoreId {get;set;}
}