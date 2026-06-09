using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Data.Enums;

namespace MyThings.Core.Entities;

public class Domain : BaseEntity
{
    public string Name { get; set; } =null!;
    public virtual ICollection<PartnerDomain> PartnerDomains { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];

    internal class DomainConfiguration : BaseEntityConfiguration<Domain>
    {
        public override void Configure(EntityTypeBuilder<Domain> builder)
        {
            base.Configure(builder);

            builder.ToTable("Domain");

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}