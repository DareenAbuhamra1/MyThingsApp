using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Core.Interfaces;

namespace MyThings.Core.Entities;

public class Driver : User
{

    public bool IsOnline { get; set; }
    public bool IsAssigned { get; set; }
    public string VehicleLicense { get; set; } = null!;
    public string DriverLicense {get;set;} = null!;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public virtual ICollection<Order> Orders {get;set;} =[];

    internal class DriverConfiguration : BaseEntityConfiguration<Driver>
    {
        public DriverConfiguration() : base(isDerived: true) { }
        public override void Configure(EntityTypeBuilder<Driver> builder)
        {
            base.Configure(builder);

            builder.ToTable("Driver");

            builder.Property(e => e.IsOnline)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.IsAssigned)
                .HasDefaultValue(false)
                .IsRequired();
            
            builder.Property(e => e.VehicleLicense)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.DriverLicense)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Latitude)
            .HasPrecision(8, 6);

            builder.Property(e => e.Longitude)
               .HasPrecision(9, 6);
        }
    }
}