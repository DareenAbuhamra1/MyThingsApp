using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;
using MyThings.Core.Enums.GenderEnum;

namespace MyThings.Core.DTOs;

public class RegisterRequestDto
{
    // when IsRegistered from AuthResultDto is false
    [Required(ErrorMessage = "First name is required.")]
    public string FirstName {get;set;} = null!;
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName {get;set;} = null!;
    [Required(ErrorMessage = "Phone number is required.")]
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [MinLength(10, ErrorMessage = "Phone number must be at least 10 digits.")]
    public string Phone { get; set; } = null!;
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    public string? Email {get;set;}
    [Required(ErrorMessage = "Role is required.")]
    //public RoleEnum Role {get;set;}
    public GenderEnum Gender {get;set;}
    public DateOnly? DateOfBirth { get; set; }
}
