using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;
public class OrderLineOption : BaseEntity
{
    public int OrderLineId { get; set; }
    public virtual OrderLine OrderLine { get; set; } = null!;
    public int ProductOptionId { get; set; }
    public virtual ProductOption ProductOption { get; set; } = null!;
    public string Option {get;set;} = string.Empty;
    public decimal Price {get;set;}
    public int Quantity {get;set;}


    internal class OrderLineOptionConfiguration : BaseEntityConfiguration<OrderLineOption>
    {
        public override void Configure(EntityTypeBuilder<OrderLineOption> builder)
        {
            base.Configure(builder);

            builder.ToTable("OrderLineOption");

            builder.Property(e => e.Option).IsRequired();
            builder.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,3)");
            builder.Property(e => e.Quantity).IsRequired();
        }
    }
}