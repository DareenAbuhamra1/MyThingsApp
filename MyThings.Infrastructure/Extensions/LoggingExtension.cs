using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Loki;
using Serilog.Sinks.Loki.Labels;
using Serilog.Sinks.OpenTelemetry;
namespace MyThings.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static void AddSharedSerilogConfiguration(this WebApplicationBuilder builder, string applicationName)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Initializing the Serilog for application {applicationName}",applicationName);

        // use serilog for logging using ILogger<>
        builder.Host.UseSerilog((context, services, configuration) => configuration
            .ReadFrom.Configuration(context.Configuration) //check appSetting.json configuration 
            .ReadFrom.Services(services) //what does this do??
            .Enrich.FromLogContext() // what does this also do??
            .Enrich.WithProperty("Application",applicationName) // attaches the application name as a constant to every log entry
            .WriteTo.Console(outputTemplate:"[{timestamp:HH:mm:ss} {level:u3} ({Application}) {Message:lj}{NewLine}{Exception}]")
            .WriteTo.File($"Log/{applicationName.ToLower()}-log-.txt",rollingInterval: RollingInterval.Day)
            .WriteTo.OpenTelemetry(options =>
            {
                // Fix: Pull explicitly using the lowercase "Telemetry:otlp" path from your appsettings
                options.Endpoint = context.Configuration.GetValue<string>("Telemetry:otlp") ?? "http://localhost:4317";
                options.Protocol = OtlpProtocol.Grpc;
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    ["service.name"] = applicationName.ToLower(),
                    ["deployment.environment"] = context.HostingEnvironment.EnvironmentName.ToLower()
                };
            }));
    }
}