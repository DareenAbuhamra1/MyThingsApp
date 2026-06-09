using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class PartnerDomain :BaseEntity{
    public int DomainId {get;set;}
 
    public int PartnerId {get;set;}

    public virtual Partner Partner {get;set;} = null!;
    public virtual Domain Domain {get;set;} = null!;

    internal class PartnerDomainConfiguration : BaseEntityConfiguration<PartnerDomain>
    {
        public override void Configure(EntityTypeBuilder<PartnerDomain> builder)
        {
            base.Configure(builder);

            builder.ToTable("PartnerDomain");

            builder.HasOne(d => d.Domain)
                .WithMany(sd => sd.PartnerDomains)
                .HasForeignKey(sd => sd.DomainId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Partner)
                .WithMany(sd => sd.PartnerDomains)
                .HasForeignKey(sd => sd.PartnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}