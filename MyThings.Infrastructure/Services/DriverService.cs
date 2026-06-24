using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Core.Wrappers;
using MyThings.Infrastructure.Mappers;

namespace MyThings.Infrastructure.Services;

public class DriverService : IDriverService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IReadUnitOfWork _readUnitOfWork;
    private readonly DriverInfoMapper _driverInfoMapper;

    public DriverService(IUnitOfWork unitOfWork,IReadUnitOfWork readUnitOfWork, DriverInfoMapper driverInfoMapper)
    {
        _unitOfWork = unitOfWork;
        _readUnitOfWork = readUnitOfWork;
        _driverInfoMapper = driverInfoMapper;
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
        var drivers = await _readUnitOfWork.Drivers.GetAllAsync();

        if(drivers == null) return ServiceResponse<IReadOnlyList<DriverInfoDto>>.Failure("No drivers found",404);

        var DriversInfoList = drivers.Select(d => _driverInfoMapper.Map(d)).ToList();

        return ServiceResponse<IReadOnlyList<DriverInfoDto>>.Ok(DriversInfoList);
    }

    public async Task<IReadOnlyList<DriverInfoDto>> GetDriversWithExpiredLicenseAsync()
    {
        var drivers = _readUnitOfWork.Drivers.GetQueryable()
            .Where(d => d.DriverLicenseExpiry <= DateTime.UtcNow)
            .Select(d => _driverInfoMapper.Map(d));

        return await drivers.ToListAsync();
    }
}