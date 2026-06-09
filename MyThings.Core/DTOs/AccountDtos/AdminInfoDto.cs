using MyThings.Core.Enums;
using MyThings.Core.Enums.GenderEnum;

namespace MyThings.Core.DTOs;

public class AdminInfoDto
{
    public int? Id {get;set;}
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public GenderEnum Gender { get; set; }
    public RoleEnum Role {get;set;}
    public bool IsActive { get; set; }
    public string EmployeeId { get; set; } = null!;
    public int JobId {get;set;}
    public string Department { get; set; } = null!; 
    public string PasswordHash { get; set; } = null!;
}