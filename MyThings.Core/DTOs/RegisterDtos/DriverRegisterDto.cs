using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class DriverRegisterDto :RegisterRequestDto{

    [Required(ErrorMessage = "Vehicle License is required.")]
    public required string VehicleLicense { get; set; } 
    
    [Required(ErrorMessage = "Driver License is required.")]
    public required string DriverLicense {get;set;} 
}