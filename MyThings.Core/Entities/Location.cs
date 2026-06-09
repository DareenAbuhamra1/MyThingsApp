using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Data.Enums;

namespace MyThings.Core.Entities;

public class Location : BaseEntity
{
    public string Title { get; set; } = null!;
    public CountryEnum Country { get; set; }
    public CityEnum City { get; set; }
    public string Area { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string? BuildingNo { get; set; }
    public string? ApartmentNo { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public string? Description { get; set; }
    public bool IsDefault { get; set; }
    public int? CustomerId { get; set; }
    public virtual Customer? Customer {get;set;}
    public virtual Partner? Partner {get;set;}

    internal class LocationConfiguration : BaseEntityConfiguration<Location>
    {
        public override void Configure(EntityTypeBuilder<Location> builder)
        {
            base.Configure(builder);

            builder.ToTable("Location");

            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Country)
                .IsRequired();

            builder.Property(e => e.City)
                .IsRequired();

            builder.Property(e => e.Area)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Street)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.BuildingNo)
                .HasMaxLength(5);

            builder.Property(e => e.ApartmentNo)
                .HasMaxLength(5);

            builder.Property(e => e.Description)
                .HasMaxLength(255);

            builder.Property(e => e.IsDefault)
                .HasDefaultValue(true);

            builder.Property(e => e.Latitude)
                .HasPrecision(8, 6)
                .IsRequired();

            builder.Property(e => e.Longitude)
                .HasPrecision(9, 6)
                .IsRequired();
        }
    }
}