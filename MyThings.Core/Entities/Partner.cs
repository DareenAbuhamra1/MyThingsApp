using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Core.Interfaces;

namespace MyThings.Core.Entities;

public class Partner : User, IActivatable
{
       public string Name { get; set; } = null!;
       public string RegistrationNo { get; set; } = null!;
       public decimal CommissionRate { get; set; }
       public int LocationId { get; set; }
       public bool IsAvailable { get; set; }
       public int? ParentStoreId { get; set; }
       public int DeliveryRuleId { get; set; }
       public bool IsManuallyClosed {get;set;}
       public virtual DeliveryRule DeliveryRule { get; set; } = null!;
       public virtual Partner? ParentStore { get; set; }
       public virtual Location Location { get; set; } = null!;
       public virtual ICollection<Product> Products { get; set; } = [];
       public virtual ICollection<Partner> InverseParentStore { get; set; } = [];
       public virtual ICollection<WorkingHour> WorkingHours { get; set; } = [];
       public virtual ICollection<Order> Orders { get; set; } = [];
       public virtual ICollection<PartnerDomain> PartnerDomains { get; set; } = [];
       public virtual ICollection<PartnerCategory> PartnerCategories { get; set; } = [];

       internal class PartnerConfiguration : BaseEntityConfiguration<Partner>
       {
              public PartnerConfiguration() : base(isDerived: true) { }
              public override void Configure(EntityTypeBuilder<Partner> builder)
              {
                     base.Configure(builder);

                     builder.ToTable("Partner");

                     builder.Property(p => p.Name)
                        .IsRequired()
                        .HasMaxLength(200);

                     builder.Property(p => p.RegistrationNo)
                            .IsRequired()
                            .HasMaxLength(50);

                     builder.Property(p => p.IsManuallyClosed)
                            .IsRequired()
                            .HasDefaultValue(true);

                     builder.Property(p => p.CommissionRate)
                            .HasColumnType("decimal(5,2)");

                     builder.HasOne(p => p.ParentStore)
                            .WithMany(p => p.InverseParentStore)
                            .HasForeignKey(p => p.ParentStoreId)
                            .OnDelete(DeleteBehavior.Restrict);

                     builder.HasOne(p => p.Location)
                            .WithOne(l => l.Partner)
                            .HasForeignKey<Partner>(p => p.LocationId)
                            .OnDelete(DeleteBehavior.Restrict);

                     builder.HasMany(p => p.Orders)
                            .WithOne(o => o.Partner)
                            .HasForeignKey(o => o.PartnerId);

                     builder.HasOne(p => p.DeliveryRule)
                        .WithMany()
                        .HasForeignKey(p => p.DeliveryRuleId)
                        .OnDelete(DeleteBehavior.Restrict);
              }
       }
}

