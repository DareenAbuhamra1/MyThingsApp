using Microsoft.AspNetCore.Http.HttpResults;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;

namespace MyThings.Infrastructure.Services;

public class DriverService : IDriverService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadUnitOfWork _readUnitOfWork;

    public DriverService(IUnitOfWork unitOfWork,IReadUnitOfWork readUnitOfWork)
    {
        _unitOfWork = unitOfWork;
        _readUnitOfWork = readUnitOfWork;
    }

    public async Task<Driver?> ActivateDriverAsync(int driverId, bool active)
    {
        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);
        if(driver == null) return null;

        driver.IsActive = active;
        _unitOfWork.Drivers.Update(driver);
        await _unitOfWork.CompleteAsync();

        return driver;
    }

    public async Task<bool> UpdateLiveLocationAsync(int driverId, decimal latitude, decimal longitude)
    {
        var driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);

        if(driver == null) return false;

        driver.Latitude = latitude;
        driver.Longitude = longitude;
        driver.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Drivers.Update(driver);
        await _unitOfWork.CompleteAsync();

        return true;
    }
    public async Task<ServiceResponse<bool>> ToggleOnlineAsync(int driverId, bool isOnline)
    {
       var Driver = await _unitOfWork.Drivers.GetByIdAsync(driverId);

       if(Driver == null) return ServiceResponse<bool>.Failure("Driver not found", 404);

        if (isOnline != Driver.IsOnline)
        {
            Driver.IsOnline = isOnline;
        }
    
        Driver.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.Drivers.Update(Driver);
        await _unitOfWork.CompleteAsync();

        return ServiceResponse<bool>.Ok(true);
    }

    public async Task<ServiceResponse<IReadOnlyList<DriverInfoDto>>> GetAllDriversAsync()
    {
        var Drivers = await _readUnitOfWork.Drivers.GetAllAsync();

        if(Drivers == null) return ServiceResponse<IReadOnlyList<DriverInfoDto>>.Failure("No drivers found",404);

        var DriversInfoList = new List<DriverInfoDto>(); 

        foreach(var d in Drivers)
        {
            DriversInfoList.Add(new DriverInfoDto
            {
                DriverId = d.Id,
                FirstName = d.FirstName,
                LastName = d.LastName, 
                Phone = d.Phone,
                IsActive = d.IsActive,
                IsAssigned = d.IsAssigned,
                VehicleLicense = d.VehicleLicense,
                DriverLicense = d.DriverLicense 
            });
        }

        return ServiceResponse<IReadOnlyList<DriverInfoDto>>.Ok(DriversInfoList);
    }
}