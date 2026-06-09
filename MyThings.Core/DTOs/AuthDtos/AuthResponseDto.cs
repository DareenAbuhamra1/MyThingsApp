using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class AuthResponseDto
{
    public bool? IsActive {get;set;} //don't allow if false
    public bool? IsRegistered {get;set;} // don't allow if false
    public bool IsSuccess {get;set;} 
    public bool RequiresOtp { get; set; }


    public string Token { get; set; } = string.Empty;
    public string RefreshToken {get;set;} = string.Empty;
    public DateTime? Expiry { get; set; }
    
    public string FullName { get; set; } = null!;
    public RoleEnum Role { get; set; } 
    public int? UserId { get; set; }
    public string Message {get;set;} = null!;
    
}