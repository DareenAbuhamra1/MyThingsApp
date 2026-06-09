using Microsoft.EntityFrameworkCore;
using MyThings.Auth.AuthServices;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Extensions;
using MyThings.Infrastructure.Helper;
using MyThings.Infrastructure.Repositories;
using MyThings.Infrastructure.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedSerilogConfiguration("DriverAPI");
builder.Services.AddSharedOpenTelemetryMetrics(builder.Configuration, "DriverAPI");

try
{

    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Jwt"));
    
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IDriverService, DriverService>();
    builder.Services.AddScoped<IDriverService, DriverService>();
    builder.Services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
    builder.Services.AddScoped<IDeliveryFeeService, DeliveryFeeService>();
    builder.Services.AddScoped<ITimeEstimationService, TimeEstimationService>();
    builder.Services.AddScoped<IAuditService, AuditService>(); 

    builder.Services.AddDbContext<WriteDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryWrite")));

    builder.Services.AddDbContext<ReadDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SecondaryRead"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
      builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowBlazorApp", policy =>
        {
            policy.WithOrigins("http://localhost:5213") // Replace with your Blazor app URL
                .AllowAnyMethod()
                .AllowAnyHeader() // This allows the Authorization header!
                .AllowCredentials(); // Required if you use cookies or specific auth headers
        });
    });

    var app = builder.Build();

    // 2. CONFIGURE PIPELINE
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<JWTMiddleware>();
    app.UseAuthorization();
    app.MapControllers(); // This tells the API to look in your Controllers folder
    app.UseCors("AllowBlazorApp");
    app.Run();
}
catch (Exception e)
{
    Log.Fatal(e,"The hosting application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
