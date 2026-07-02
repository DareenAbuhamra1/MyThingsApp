var builder = DistributedApplication.CreateBuilder(args);


// Add Customer API project to the distributed application after referencing it in the MyThings.AppHost.csproj file
var customerApi = builder.AddProject<Projects.MyThings_API_Customer>("customer-api");

var adminApi = builder.AddProject<Projects.MyThings_API_Admin>("admin-api")
    .WithReference(customerApi);
    
var driverApi = builder.AddProject<Projects.MyThings_API_Driver>("driver-api");
var partnerApi = builder.AddProject<Projects.MyThings_API_Partner>("partner-api");

builder.Build().Run();
