using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Mythings.Core.Interaces.Repositories;
using Mythings.Core.Interaces.Services;
using MyThings.Auth.AuthServices;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure;
using MyThings.Infrastructure.BackgroundWorkers;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Extensions;
using MyThings.Infrastructure.Helper;
using MyThings.Infrastructure.Repositories;
using MyThings.Infrastructure.Services;
using Serilog;
using Hangfire;
using Hangfire.SqlServer;
using Hangfire.Common;
using MyThings.Core.Interfaces.Services;
using MyThings.Infrastructure.Mappers;
using MyThings.Core.DTOs;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedSerilogConfiguration("AdminAPI");
builder.Services.AddSharedOpenTelemetryMetrics(builder.Configuration, "AdminAPI");

try
{
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Jwt"));

    // 1. ADD SERVICES (The "Underlying" parts)
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<ILogisiticService, LogisticService>();
    builder.Services.AddScoped<IProductService, ProductService>();
    builder.Services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
    builder.Services.AddScoped<IDomainService, DomainService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IDriverService, DriverService>();
    builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
    builder.Services.AddScoped<IDeliveryFeeService, DeliveryFeeService>();
    builder.Services.AddScoped<ITimeEstimationService, TimeEstimationService>();
    builder.Services.AddScoped<IWorkingHoursService, WorkingHoursService>();
    builder.Services.AddScoped<IPartnerRepository, PartnerRepository>();
    builder.Services.AddScoped<IWorkingHoursRepository, WorkingHoursRepository>();
    builder.Services.AddScoped<IAuditService, AuditService>();
    builder.Services.AddScoped<IUserAccessor, UserAccessor>();
    builder.Services.AddScoped<IJobService, JobService>();
    builder.Services.AddScoped<IPartnerService, PartnerService>();
    builder.Services.AddScoped<IPartnerReadRepository, PartnerReadRepository>();
    builder.Services.AddScoped<ICustomerAdminService, CustomerAdminService>();
    
    builder.Services.AddSingleton<PartnerOrderMapper>();
    builder.Services.AddSingleton<OrderPaginationMapper>();
    builder.Services.AddSingleton<OrderInfoMapper>();
    builder.Services.AddSingleton<OrderDetailedMapper>();
    builder.Services.AddSingleton<OrderPlacementResponseMapper>();
    builder.Services.AddSingleton<OrderCartResponseMapper>();
    builder.Services.AddSingleton<AdminOrderMapper>();
    builder.Services.AddSingleton<OrderCartViewMapper>();
    builder.Services.AddSingleton<AdminOrderResponseMapper>();
    builder.Services.AddSingleton<DriverAssignedOrderMapper>();
    builder.Services.AddSingleton<CutomerLocationMapper>();
    builder.Services.AddSingleton<LocationMapper>();
    builder.Services.AddSingleton<DriverInfoMapper>();
    builder.Services.AddSingleton<ProductOptionDisplayMapper>();
    builder.Services.AddSingleton<ProductDisplayMapper>();
    builder.Services.AddSingleton<StoreDisplayMapper>();
    builder.Services.AddSingleton<CustomerAdminMapper>();

    builder.Services.AddHangfireServer();
    
    var connectionString = builder.Configuration.GetConnectionString("PrimaryWrite");

    builder.Services.AddDbContext<WriteDbContext>(
        options => options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        }));

    builder.Services.AddDbContext<ReadDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SecondaryRead"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularUI", policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader() // This allows the Authorization header!
                .AllowCredentials() // Required if I use cookies or specific auth headers
                .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
        });
    });

    builder.Services.AddHangfire(config =>
    {
        config.UseSqlServerStorage(
            connectionString);
    });

    builder.Services.AddHostedService<PartnerAvailabilityWorker>();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        });

   

    var app = builder.Build();


    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    /*
    string passwordHash = BCrypt.Net.BCrypt.HashPassword("AIYGIWGWYAIYDIWDWY@SYS26");
    Console.WriteLine(passwordHash);
    */
    app.UseHttpsRedirection();
    app.UseCors("AllowAngularUI");
    app.UseMiddleware<JWTMiddleware>();
    app.UseAuthorization();
    app.MapControllers(); // This tells the API to look in your Controllers folder
    app.UseHangfireDashboard();

    HangfireJobSchedular.ScheduleJob();

    app.Run();

}
catch (Exception e)
{
    Log.Fatal(e, "The application host terminated unexpectdly !");
}
finally
{
    //clean up connection and flush log streams before complete shutdown
    Log.CloseAndFlush();
}
