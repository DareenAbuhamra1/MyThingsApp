using Microsoft.EntityFrameworkCore;
using Mythings.Core.Interaces.Services;
using Mythings.Infrastructure.Helper;
using MyThings.Auth.AuthServices;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Core.Interfaces.Services;
using MyThings.Infrastructure.Context;
using MyThings.Infrastructure.Extensions;
using MyThings.Infrastructure.Helper;
using MyThings.Infrastructure.Mappers;
using MyThings.Infrastructure.Repositories;
using MyThings.Infrastructure.Services;
using Serilog;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddSharedSerilogConfiguration("CustomerAPI");
builder.Services.AddSharedOpenTelemetryMetrics(builder.Configuration, "CustomerAPI");

try
{
    builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("Jwt"));


    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddMemoryCache();

    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<ITokenService, TokenService>();
    builder.Services.AddScoped<IReadUnitOfWork, ReadUnitOfWork>();
    builder.Services.AddScoped<IPartnerReadRepository, PartnerReadRepository>();
    builder.Services.AddScoped<ICustomerPartnerService, CustomerPartnerService>();
    builder.Services.AddScoped<IDomainService, DomainService>();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IOrderReadRepository, OrderReadRepository>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();
    builder.Services.AddScoped<IDeliveryFeeService, DeliveryFeeService>();
    builder.Services.AddScoped<ILocationService, LocationService>();
    builder.Services.AddScoped<ITimeEstimationService, TimeEstimationService>();
    builder.Services.AddScoped<IAuditService, AuditService>();
    builder.Services.AddScoped<IPartnerService, PartnerService>();
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
    builder.Services.AddSingleton<RedisCacheService>();
    builder.Services.AddSingleton<HybridCacheService>();

    builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
    {
        var config = builder.Configuration.GetSection("Redis:ConnectionString").Value;
        return ConnectionMultiplexer.Connect(config!);
    });
    
    builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration["Redis:ConnectionString"];
        }
    );
    builder.Services.AddHybridCache();
    
    builder.Services.AddDbContext<WriteDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("PrimaryWrite")));

    builder.Services.AddDbContext<ReadDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SecondaryRead"))
        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));
    
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowAngularUI", policy =>
        {
            policy.WithOrigins("http://localhost:4200") // Replace with your Blazor app URL
                .AllowAnyMethod()
                .AllowAnyHeader() // This allows the Authorization header!
                .AllowCredentials(); // Required if you use cookies or specific auth headers
        });
    });
    
    builder.Services.Configure<RabbitMqSettings>(
        builder.Configuration.GetRequiredSection("RabbitMQ")
    );
    // builder.Services.AddSharedAuth(builder.Configuration);

    var app = builder.Build();

    // 2. CONFIGURE PIPELINE
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseMiddleware<JWTMiddleware>();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers(); // This tells the API to look in your Controllers folder
    app.UseCors("AllowAngularUI");
    
    app.Run();

}
catch (Exception e)
{
    Log.Fatal(e, "The application host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

