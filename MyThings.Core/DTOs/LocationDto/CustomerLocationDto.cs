using System.ComponentModel.DataAnnotations;
using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;

public class CustomerLocationDto
{
    public required int CustomerId {get;set;}
    public int? LocationId {get;set;}
    public string AddressTitle { get; set; } = "Home"; 
    [Required(ErrorMessage = "Country is required.")]
    public CountryEnum Country { get; set; }
    [Required(ErrorMessage = "City is required.")]
    public CityEnum City { get; set; }
    [Required(ErrorMessage = "Area is required.")]
    public string Area { get; set; } = string.Empty;
    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; } = string.Empty;
    public string? BuildingNo {get;set;}
    public string? ApartmentNo {get;set;}
    [Required(ErrorMessage = "Latitude is required.")]
    public decimal Latitude { get; set; }
    [Required(ErrorMessage = "Longitude is required.")]
    public decimal Longitude { get; set; } 
    public string? Description { get; set; }
}
 