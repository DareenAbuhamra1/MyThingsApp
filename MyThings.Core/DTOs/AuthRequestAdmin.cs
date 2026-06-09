using System.ComponentModel.DataAnnotations;

namespace MyThings.Core.DTOs;

public class AuthRequestAdmin
{
    [Required]
    public string Email {get;set;} = string.Empty;
    [Required]
    public string Password {get;set;} = string.Empty;
}