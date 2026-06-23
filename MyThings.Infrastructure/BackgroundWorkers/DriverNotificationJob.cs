using Microsoft.Extensions.Logging;
using MyThings.Core.DTOs;

namespace MyThings.Infrastructure.BackgroundWorkers;

public class DriverNotificationJob
{
    private readonly ILogger<DriverNotificationJob> _logger;
    public DriverNotificationJob(ILogger<DriverNotificationJob> logger)
    {
        _logger = logger;
    }
    public void NotifyDrivers(IEnumerable<DriverInfoDto> drivers)
    {
        foreach (DriverInfoDto d in drivers)
        {
            _logger.LogInformation($"Driver {d.Id} with License No. {d.DriverLicense} expired on {d.DriverLicenseExpiry}");
        }
        
    }
}