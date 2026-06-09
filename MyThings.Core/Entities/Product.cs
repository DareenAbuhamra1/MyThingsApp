using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class Product : BaseEntity
{
    public int PartnerId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    //public decimal SalePrice { get; set; }
    public int Stock { get; set; }
    public string? Description { get; set; }
    public virtual ICollection<OrderLine> OrderLines { get; set; } = [];
    public virtual ICollection<OptionGroup> OptionGroups { get; set; } = [];
    public virtual Partner Partner { get; set; } = null!;

    internal class ProductCongfiguration : BaseEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);

            builder.ToTable("Product");

            builder.Property(e => e.Name)
            .HasMaxLength(255)
            .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnType("decimal(10,3)")
                .IsRequired();

            builder.Property(e => e.Price)
                .HasColumnType("decimal(10,3)");

            builder.Property(e => e.Stock)
                .HasDefaultValue(0);

            builder.Property(e => e.Description)
                .HasMaxLength(255);

            builder.HasOne(s => s.Partner)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.PartnerId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}