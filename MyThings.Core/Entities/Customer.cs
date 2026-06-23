using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MyThings.Core.Entities;

public class Customer : User
{
    public decimal WalletBalance { get; set; }
    
    // Profile and Status Fields
    public int TypeId { get; set; }
    public int CustomerStatusId { get; set; }
    public int LanguageId { get; set; }
    
    // Location Fields
    public int? CountryId { get; set; }
    public int? CityId { get; set; }
    
    // Media and Search Fields
    public int? MediaId { get; set; }
    
    // Multi-tenancy and Availability
    public string TenantId { get; set; } = "default";
    public int AvailabilityId { get; set; }
    
    // Navigation Properties
    public virtual ICollection<Location> Locations { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];
    public virtual CustomerStatus? CustomerStatus { get; set; }
    public virtual Language? Language { get; set; }
    public virtual Media? Media { get; set; }

    internal class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public CustomerConfiguration() : base(isDerived: true) { }
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("Customer");

            builder.Property(e => e.WalletBalance)
                .HasColumnType("decimal(18,3)")
                .HasDefaultValue(0.0m);

            // Profile and Status Configuration
            builder.Property(e => e.TypeId)
                .IsRequired()
                .HasDefaultValue(1);

            builder.Property(e => e.CustomerStatusId)
                .IsRequired();

            builder.Property(e => e.LanguageId)
                .IsRequired()
                .HasDefaultValue(1);

            // Location Configuration
            builder.Property(e => e.CountryId);

            builder.Property(e => e.CityId);

            // Multi-tenancy Configuration
            builder.Property(e => e.TenantId)
                .HasMaxLength(50)
                .IsRequired()
                .HasDefaultValue("default");

            builder.Property(e => e.AvailabilityId)
                .IsRequired()
                .HasDefaultValue(1);

            // Foreign Key Relationships
            builder.HasOne(c => c.CustomerStatus)
                .WithMany()
                .HasForeignKey(c => c.CustomerStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Language)
                .WithMany()
                .HasForeignKey(c => c.LanguageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Media)
                .WithMany()
                .HasForeignKey(c => c.MediaId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // Indexes for performance
            builder.HasIndex(c => c.TenantId);
            builder.HasIndex(c => c.AvailabilityId);
            builder.HasIndex(c => c.CustomerStatusId);
            builder.HasIndex(c => c.LanguageId);
            builder.HasIndex(c => new { c.TenantId, c.AvailabilityId, c.CustomerStatusId });
        }
    }
}