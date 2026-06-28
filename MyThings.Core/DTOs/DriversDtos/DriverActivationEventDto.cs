namespace MyThings.Core.DTOs;
public class DriverActivationEventDto
{
    public int DriverId { get; set; } 
    public int? AdminId { get; set; } = 1;
    public string DriverPhone { get; set; } = string.Empty;
    public bool Active {get;set;}
}