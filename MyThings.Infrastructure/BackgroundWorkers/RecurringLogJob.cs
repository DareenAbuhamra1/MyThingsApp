using Microsoft.Extensions.Logging;

public class RecurringLogJob
{
    private readonly ILogger<RecurringLogJob> _logger;
    
    public RecurringLogJob(ILogger<RecurringLogJob> logger)
    {
        _logger = logger;
    }
    public void Run()
    {
        _logger.LogInformation($"HELLO FROM RECURRING JOB {DateTime.Now}");
    }
}