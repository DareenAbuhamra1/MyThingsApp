using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class OrderLine : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public string? Note { get; set; } = null!;
    public virtual Product? Product { get; set; }
    public virtual Order? Order { get; set; }
    public virtual ICollection<OrderLineOption> OrderLineOptions { get; set; } = [];

    internal class OrderLineConfiguration : BaseEntityConfiguration<OrderLine>
    {
        public override void Configure(EntityTypeBuilder<OrderLine> builder)
        {
            base.Configure(builder);

            builder.ToTable("OrderLine");

            builder.Property(e => e.ProductName)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.Quantity)
                .IsRequired();

            builder.Property(e => e.Price)
                .HasPrecision(10, 3)
                .IsRequired();

            builder.Property(e => e.Note)
                .HasMaxLength(255);

            builder.HasOne(p => p.Product)
                .WithMany(ol => ol.OrderLines)
                .HasForeignKey(ol => ol.ProductId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Order)
                .WithMany(ol => ol.OrderLines)
                .HasForeignKey(ol => ol.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(olo => olo.OrderLineOptions)
                .WithOne(ol => ol.OrderLine)
                .HasForeignKey(olo => olo.OrderLineId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}