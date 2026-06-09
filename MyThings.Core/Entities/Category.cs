using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class Category : BaseEntity
{
    public int DomainId {get;set;}
    public virtual Domain? Domain {get;set;}
    public string Name {get;set;} = null!;
    public virtual ICollection<PartnerCategory> PartnerCategories {get;set;} =[];

    internal class CategoryConfiguration : BaseEntityConfiguration<Category>
    {
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            base.Configure(builder);
            
            builder.ToTable("Category");

            builder.Property(e => e.Name)
                .IsRequired();

            builder.HasOne(d => d.Domain)
                .WithMany()
                .HasForeignKey(d => d.DomainId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}