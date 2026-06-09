using Microsoft.EntityFrameworkCore;
using MyThings.Core.Entities;
using MyThings.Core.Enums;
using MyThings.Core.Enums.GenderEnum;

namespace MyThings.Infrastructure.Context;

public class WriteDbContext : BaseDbContext
{
    public WriteDbContext(DbContextOptions<WriteDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seedDate = new DateTime(2026, 5, 3, 0, 0, 0, DateTimeKind.Utc);

        modelBuilder.Entity<Job>().HasData(new
        {
            Id = 1,
            Title = "Super Admin",
            CanManageAccounts = true,
            CanManageLogistics = true,
            CanManageProducts = true,
            CreatedAt = seedDate,
        });
        modelBuilder.Entity<Admin>().HasData(new
        {
            //User properties
            Id = 1,
            FirstName = "Dareen",
            LastName = "Abuhamra",
            Phone = "0790000000",
            Email = "superadmin@mythings.app",
            Gender = GenderEnum.Female,
            Role = RoleEnum.SuperAdmin,
            DateOfBirth = new DateOnly(2004, 4, 8),
            CreatedAt = seedDate,
            IsActive = true,

            //Admin properties
            EmployeeId = "5000",
            JobId = 1,
            Department = "Operations",
            PasswordHash = "$2a$11$KscYwNeQTX2GmzSJiHQBmu.WUcm7.m7FThl7i/sQUJdw7PGaz8ywO",
        });
    }
}
