using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MyThings.Core.Entities;

public class Customer : User
{
    public decimal WalletBalance { get; set; }
    public virtual ICollection<Location> Locations { get; set; } = [];
    public virtual ICollection<Order> Orders { get; set; } = [];

    internal class CustomerConfiguration : BaseEntityConfiguration<Customer>
    {
        public CustomerConfiguration() : base(isDerived: true) { }
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);

            builder.ToTable("Customer");

            builder.Property(e => e.WalletBalance)
                .HasColumnType("decimal(18,3)")
                .HasDefaultValue(0.0m);

        }

    }
}