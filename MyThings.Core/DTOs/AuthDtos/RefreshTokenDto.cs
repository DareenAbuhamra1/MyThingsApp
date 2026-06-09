using System.ComponentModel.DataAnnotations;

namespace MyThings.Core.DTOs;
public class RefreshTokenDto
{
    [Required(ErrorMessage = "Expired token is required.")]
    public required string ExpiredToken {get;set;}           
    [Required(ErrorMessage = "Refresh token is required.")]
    public string RefreshToken {get;set;} = null!;
}