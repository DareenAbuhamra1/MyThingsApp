using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class OptionGroup : BaseEntity
{

    public int ProductId { get; set; }

    public string Title { get; set; } = null!;

    public bool IsRequired { get; set; }

    public int MinSelection { get; set; }

    public int MaxSelection { get; set; }
    public virtual Product Product { get; set; } = null!;
    public virtual ICollection<ProductOption>? ProductOptions { get; set; } = [];

    internal class OptionGroupConfiguration : BaseEntityConfiguration<OptionGroup>
    {
        public override void Configure(EntityTypeBuilder<OptionGroup> builder)
        {
            base.Configure(builder);

            builder.ToTable("OptionGroup");

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.IsRequired)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.MinSelection)
                .IsRequired();

            builder.Property(e => e.MaxSelection)
                .IsRequired();

            builder.HasOne(og => og.Product)
                .WithMany(p => p.OptionGroups)
                .HasForeignKey(og => og.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}