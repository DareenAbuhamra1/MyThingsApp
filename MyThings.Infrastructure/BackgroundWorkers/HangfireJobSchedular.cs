using Hangfire;

namespace MyThings.Infrastructure.BackgroundWorkers;

public static class HangfireJobSchedular
{
    public static void ScheduleJob()
    {
        RecurringJob.AddOrUpdate<RecurringLogJob>(
            "log-job",
            j => j.Run(),
            Cron.Minutely
        );
        RecurringJob.AddOrUpdate<DriverLicenseExpiryJob>(
            "driver-license-expiry-job",
            j => j.CheckDriverLicenseExpiry(),
            Cron.Minutely
        );
    }
}