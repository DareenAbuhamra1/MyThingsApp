using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Wrappers;

namespace MyThings.Core.Interfaces;

public interface IDriverService
{
    Task<bool> UpdateLiveLocationAsync(int driverId, decimal latitude, decimal longitude);
    //Task<bool> IsNearStoreAsync(int driverId, int storeId);
    Task<Driver?> ActivateDriverAsync(int driverId, bool active);
    Task<ServiceResponse<bool>> ToggleOnlineAsync(int driverId, bool isOnline);
    Task<ServiceResponse<IReadOnlyList<DriverInfoDto>>> GetAllDriversAsync();
}
