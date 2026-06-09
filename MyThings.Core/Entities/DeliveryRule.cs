using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Data.Enums;

namespace MyThings.Core.Entities;

public class DeliveryRule : BaseEntity
{
    public CityEnum City { get; set; } // e.g., "Amman", "Irbid" 
    public decimal BaseFee { get; set; } // The starting price (e.g., 2.00 JOD)
    public decimal PerKmFee { get; set; } // Extra cost per kilometer
    public decimal MinTotalForFreeDelivery { get; set; } // e.g., 20.00 JOD

    internal class DeliveryRuleConfiguration : BaseEntityConfiguration<DeliveryRule>
    {
        public override void Configure(EntityTypeBuilder<DeliveryRule> builder)
        {
            base.Configure(builder);

            builder.ToTable("DeliveryRule");

            builder.Property(dr => dr.BaseFee).HasColumnType("decimal(18,3)");
            builder.Property(dr => dr.PerKmFee).HasColumnType("decimal(18,3)");
            builder.Property(dr => dr.MinTotalForFreeDelivery).HasColumnType("decimal(18,3)");

           
            builder.Property(dr => dr.City)
                .IsRequired();

            builder.HasMany<Partner>()
                   .WithOne(p => p.DeliveryRule)
                   .HasForeignKey(p => p.DeliveryRuleId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }

}