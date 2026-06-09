using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class ProductOption : BaseEntity
{
    public int OptionGroupId { get; set; }
    public string Option { get; set; } = null!;
    public decimal Price { get; set; }
    public virtual OptionGroup? OptionGroup { get; set; }

    internal class ProductOptionConfiguration : BaseEntityConfiguration<ProductOption>
    {
        public override void Configure(EntityTypeBuilder<ProductOption> builder)
        {
            base.Configure(builder);
            builder.ToTable("ProductOption");

            builder.Property(e => e.Option)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnType("decimal(10,3)")
                .HasDefaultValue(0)
                .IsRequired();

            builder.HasOne(po => po.OptionGroup)
                .WithMany(og => og.ProductOptions)
                .HasForeignKey(po => po.OptionGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}