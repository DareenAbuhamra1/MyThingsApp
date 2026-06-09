using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;
public class VerifyOtpRequestDto 
{ 
    [Required(ErrorMessage = "Phone number is required")]
    public string Phone { get; set; } = null!;
    [Required(ErrorMessage = "OTP code is required")]
    public string OtpCode { get; set; } = null!;
    [Required(ErrorMessage = "Role is required")]
    public RoleEnum Role { get; set; }
}