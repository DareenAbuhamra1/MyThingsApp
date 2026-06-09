using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyThings.Data.Enums;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.BackgroundWorkers;

public class PartnerAvailabilityWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PartnerAvailabilityWorker> _logger;

    public PartnerAvailabilityWorker(IServiceProvider serviceProvider,ILogger<PartnerAvailabilityWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(TimeSpan.FromMinutes(3));
        _logger.LogInformation("Relational Partner Availability Worker initialized.");

        while (await timer.WaitForNextTickAsync(stoppingToken) && !stoppingToken.IsCancellationRequested)
        {
            try
            {
                await EvaluateAvailabilityAsync();
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error processing background availability transitions.");
            }
        }  
    }
    private async Task EvaluateAvailabilityAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<WriteDbContext>();

        var JorTimeZoneStr = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)? "Jordan Standard Time": "Asia/Amman";

        var JordanTimeZone = TimeZoneInfo.FindSystemTimeZoneById(JorTimeZoneStr);
        var JodanDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow,JordanTimeZone);

        //TimeOnly CurrentLocalTime = TimeOnly.FromDateTime(JodanDateTime);
        //DayOfWeek LocalDayOfWeek = JodanDateTime.DayOfWeek;

        DayEnum TodayEnum = (DayEnum)JodanDateTime.DayOfWeek;

        TimeOnly currentUtcTime = TimeOnly.FromDateTime(DateTime.UtcNow);

        _logger.LogInformation("System Wake Cycle executed. Current Amman Schedule: Day={day} ({index}), Time={time}", 
        TodayEnum, (int)TodayEnum, currentUtcTime);

        var Partners = await context.Partners
            .Include(p => p.WorkingHours)
            .ToListAsync();

        foreach(var p in Partners)
        {
            bool shouldBeAvailable = false;

            var todaysShift = p.WorkingHours
                .FirstOrDefault(w => w.Day == TodayEnum);

            if(todaysShift != null )
            {
               TimeOnly begin = todaysShift.ShiftBegin;
               TimeOnly end = todaysShift.ShiftEnd;

                if(begin < end)
                {
                    shouldBeAvailable = currentUtcTime >= begin && currentUtcTime <= end;
                }
                if(begin > end)
                {
                    shouldBeAvailable = currentUtcTime >= begin || currentUtcTime <= end;
                }
            }

            if (p.IsManuallyClosed || !p.IsActive)
            {
                shouldBeAvailable = false;
            }

            if(p.IsAvailable != shouldBeAvailable)
            {
                p.IsAvailable = shouldBeAvailable;
                _logger.LogInformation("Database State Mutated: Store '{name}' transitioned availability to: {status}", 
                p.Name, shouldBeAvailable);
            }
        };
        await context.SaveChangesAsync();
    }
}