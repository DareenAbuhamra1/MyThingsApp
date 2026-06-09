using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;
using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;

public class PartnerRegisterDto : RegisterRequestDto{
    [Required]
    public required string Name {get;set;}
    [Required]
    public required string RegistrationNo { get; set; } 
    [Required]
    public required decimal CommissionRate { get; set; }
    public int? ParentStoreId { get; set; }
    [Required]
    public required CountryEnum Country { get; set; }
    [Required]
    public required CityEnum City { get; set; }
    [Required]
    public required string Area { get; set; }
    [Required]    
    public required string Street { get; set; }
    [Required]
    public required decimal Latitude { get; set; } 
    [Required]
    public required decimal Longitude { get; set; }
    [Required]
    public required int DeliveryRuleId { get; set; }
}