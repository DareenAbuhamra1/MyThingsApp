using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

public interface IWorkingHoursRepository
{
    Task<Partner?> GetWorkingHoursAsync(int PartnerId);
    Task<bool> AddWorkingHoursAsync(int PartnerId, IEnumerable<WorkingHour> newShifts);
}