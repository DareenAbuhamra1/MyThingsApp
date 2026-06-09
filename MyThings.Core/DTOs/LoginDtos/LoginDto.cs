using System.ComponentModel.DataAnnotations;

namespace MyThings.Core.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Phone number is required.")]
    public required string Phone { get; set; } 
}