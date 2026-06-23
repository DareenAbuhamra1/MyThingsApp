using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using Hangfire;
using Microsoft.Extensions.Logging;
using Serilog;

namespace MyThings.Infrastructure.BackgroundWorkers;

public class DriverLicenseExpiryJob
{
    private readonly IDriverService _driverService;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly ILogger<DriverLicenseExpiryJob> _logger;
    public DriverLicenseExpiryJob(IDriverService driverService,IBackgroundJobClient backgroundJobClient,ILogger<DriverLicenseExpiryJob> logger)
    {
        _driverService = driverService;
        _backgroundJobClient = backgroundJobClient;
        _logger = logger;
    }

    public async Task CheckDriverLicenseExpiry()
    {
        _logger.LogInformation($"CheckDriverLicenseExpiry Job {DateTime.UtcNow}");

        var driversWithExpiredLicenseList = await _driverService.GetDriversWithExpiredLicenseAsync();
        
        if(driversWithExpiredLicenseList != null && driversWithExpiredLicenseList.Any())
        {
            _backgroundJobClient.Enqueue<DriverNotificationJob>(
                j => j.NotifyDrivers(driversWithExpiredLicenseList)
            );
        }

    }
}