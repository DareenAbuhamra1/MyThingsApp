using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class PartnerCategory : BaseEntity
{
    public int PartnerId {get;set;}
    public virtual Partner? Partner {get;set;}
    public int CategoryId {get;set;}
    public virtual Category? Category {get;set;}

    internal class PartnerCategoryConfigurtion : BaseEntityConfiguration<PartnerCategory>
    {
        public override void Configure(EntityTypeBuilder<PartnerCategory> builder)
        {
            base.Configure(builder);

            builder.ToTable("PartnerCategory");

            builder.HasOne(p => p.Partner)
                .WithMany(p => p.PartnerCategories)
                .HasForeignKey(pc => pc.PartnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Category)
                .WithMany(p => p.PartnerCategories)
                .HasForeignKey(pc => pc.CategoryId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
                
        }
    }
}