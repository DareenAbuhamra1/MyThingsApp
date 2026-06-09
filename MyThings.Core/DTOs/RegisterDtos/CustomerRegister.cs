using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;
using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;
public class CustomerRegisterDto : RegisterRequestDto
{
    // You can add customer-specific fields here if needed
    public string AddressTitle { get; set; } = "Home"; 
    [Required(ErrorMessage = "Country is required.")]
    public CountryEnum Country { get; set; }
    [Required(ErrorMessage = "City is required.")]
    public CityEnum City { get; set; }
    [Required(ErrorMessage = "Area is required.")]
    public string Area { get; set; } = string.Empty;
    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; } = string.Empty;
    [Required(ErrorMessage = "Latitude is required.")]
    public decimal Latitude { get; set; }
    [Required(ErrorMessage = "Longitude is required.")]
    public decimal Longitude { get; set; } 
    public string? Description { get; set; }
}