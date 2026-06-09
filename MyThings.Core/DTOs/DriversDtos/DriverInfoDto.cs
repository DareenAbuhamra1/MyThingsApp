namespace MyThings.Core.DTOs;

public class DriverInfoDto
{
    public int DriverId {get;set;}
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public string Phone {get;set;} = string.Empty;
    public bool IsActive {get;set;}
    public bool IsAssigned {get;set;}
    public string VehicleLicense {get;set;} = string.Empty;
    public string DriverLicense {get;set;} = string.Empty;
}