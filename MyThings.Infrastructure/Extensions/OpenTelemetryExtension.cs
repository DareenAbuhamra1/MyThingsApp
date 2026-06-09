using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace MyThings.Infrastructure.Extensions;

public static class OpenTelemetryExtension
{
    public static void AddSharedOpenTelemetryMetrics(this IServiceCollection services, IConfiguration configuration, string serviceName)
    {
        services.AddOpenTelemetry()
            .WithMetrics(metrics =>
            {
                metrics
                    .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName))
                    .AddMeter(serviceName)
                    .AddAspNetCoreInstrumentation() //automatically tracks all HTTP requests
                    .AddHttpClientInstrumentation() // when a service calls a service
                    .AddRuntimeInstrumentation() // .NET runtime metrics
                    .AddOtlpExporter(options =>
                    {
                        var configEndpoint = configuration.GetValue<string>("Telemetry:otlp") ?? "http://localhost:4317";
                        options.Endpoint = new Uri(configEndpoint);
                        options.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
                    });
            });
    }
}