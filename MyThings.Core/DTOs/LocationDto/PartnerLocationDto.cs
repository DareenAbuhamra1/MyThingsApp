namespace MyThings.Core.DTOs;

public class PartnerLocationDto
{
    public int Id {get;set;}
    public int CustomerId { get; set; }
    public string Title { get; set; } = null!;
    public string Country { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Area { get; set; } = null!;
    public string Street { get; set; } = null!;
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? BuildingNo {get;set;}
    public string? ApartmentNo {get;set;}
    public bool IsDefault { get; set; }
}