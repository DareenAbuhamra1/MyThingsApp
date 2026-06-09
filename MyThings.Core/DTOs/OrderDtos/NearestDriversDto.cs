namespace MyThings.Core.DTOs;

public class NearestDriversDto
{
    public int DriverId {get;set;}
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    public string Phone {get;set;} = string.Empty;
    public string DistanceInKm {get;set;} = string.Empty;
}