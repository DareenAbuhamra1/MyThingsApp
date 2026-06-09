using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IWorkingHoursService
{
    Task<ServiceResponse<bool>> ToggleManualCloseAsync(int PartnerId, bool IsAvailable);
    Task<ServiceResponse<WorkingHoursDto>> GetPartnerWorkingHoursAsync(int PartnerId);
    Task<ServiceResponse<bool>> AddWorkingHoursAsync(WorkingHoursDto workingHours);
}