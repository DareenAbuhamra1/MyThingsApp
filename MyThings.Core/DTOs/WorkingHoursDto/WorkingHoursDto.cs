using MyThings.Data.Enums;

namespace MyThings.Core.DTOs;


public class WorkingHoursDto
{
    public int PartnerId {get;set;}
    public List<WorkingHourDto> WorkingHours {get;set;} =[];
}

public class WorkingHourDto
{
    public required DayEnum Day {get;set;}
    public required TimeOnly ShiftBegin {get;set;}
    public required TimeOnly ShiftEnd {get;set;}
}