using System.ComponentModel.DataAnnotations;

namespace MyThings.Core.DTOs;

public class DomainCreationDto
{
    [Required]
    public required string Name { get; set; } 
}