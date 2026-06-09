using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MyThings.Infrastructure.Context;


public class ReadDbContextFactory : IDesignTimeDbContextFactory<ReadDbContext>
{
    public ReadDbContext CreateDbContext(string[] args)
    {
        var OptionsBuilder = new DbContextOptionsBuilder<ReadDbContext>();

        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../MyThings.API.Admin")) // look at this current folder
            .AddJsonFile("appsettings.json")
            .Build(); // converts JSON file to a searchable C# object

        var ConnectionString = configuration.GetConnectionString("SecondaryRead");

        OptionsBuilder.UseSqlServer(ConnectionString);

        return new ReadDbContext(OptionsBuilder.Options);
    }
}