using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MyThings.Infrastructure;


namespace MyThings.Infrastructure.Context;

public class WriteDbContextFactory : IDesignTimeDbContextFactory<WriteDbContext>
{
    public WriteDbContext CreateDbContext(string[] args)
    {
        var OptionsBuilder = new DbContextOptionsBuilder<WriteDbContext>();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyThings.API.Admin")) // look at this current folder
            .AddJsonFile("appsettings.json")
            .Build(); // converts JSON file to a searchable C# object

        var ConnectionString = configuration.GetConnectionString("PrimaryWrite");

        OptionsBuilder.UseSqlServer(ConnectionString);

        return new WriteDbContext(OptionsBuilder.Options);
    }
}
