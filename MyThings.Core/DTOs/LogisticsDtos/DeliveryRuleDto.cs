using System.ComponentModel.DataAnnotations;
using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;

public class DeliveryRuleDto
{
    [Required]
    public required CityEnum City { get; set;}
    [Required]
    public required decimal BaseFee { get; set; }
    [Required]
    public required decimal PerKmFee { get; set; }   
    [Required]
    public required decimal MinTotalForFreeDelivery { get; set; }

}