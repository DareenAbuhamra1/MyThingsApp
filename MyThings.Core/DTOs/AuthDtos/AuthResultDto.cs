using System.ComponentModel.DataAnnotations;
using MyThings.Core.Enums;

namespace MyThings.Core.DTOs;

public class AuthResultDto
{
    // result indicating if user exists or needs registration
    public bool? IsApproved { get; set; }
    public bool IsRegistered {get;set;}
    public bool IsSuccess {get;set;}
    public bool? IsAuthorized {get;set;}
    
    // if true provide token 
    public string Token { get; set; } = string.Empty;
    public string RefreshToken {get;set;} = string.Empty;
    public DateTime? Expiry { get; set; }
    //if false
    public string FullName { get; set; } = null!;
    public RoleEnum Role { get; set; } 
    public int? UserId { get; set; }
    public string Message {get;set;} = null!;
    public string VerifiedPhone {get;set;} = null!;
}