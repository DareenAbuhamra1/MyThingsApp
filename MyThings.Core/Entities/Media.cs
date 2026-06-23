using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class Media : BaseEntity
{
    public string Name { get; set; } = null!;
    public string ImageUrl { get; set; } = null!;
    public string? Color { get; set; }
    public string? TextColor { get; set; }
    public string? RoundTextColor { get; set; }
    public bool IsVideo { get; set; }
    public int DisplayOrder { get; set; }
    public string? Alt { get; set; }
    public decimal? WHRatio { get; set; }

    internal class MediaConfiguration : BaseEntityConfiguration<Media>
    {
        public override void Configure(EntityTypeBuilder<Media> builder)
        {
            base.Configure(builder);

            builder.ToTable("Media");

            builder.Property(e => e.Name)
                .HasMaxLength(255)
                .IsRequired();

            builder.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(e => e.Color)
                .HasMaxLength(20);

            builder.Property(e => e.TextColor)
                .HasMaxLength(20);

            builder.Property(e => e.RoundTextColor)
                .HasMaxLength(20);

            builder.Property(e => e.Alt)
                .HasMaxLength(255);

            builder.Property(e => e.WHRatio)
                .HasColumnType("decimal(5,2)");

            builder.Property(e => e.DisplayOrder)
                .HasDefaultValue(0);

            builder.Property(e => e.IsVideo)
                .HasDefaultValue(false);
        }
    }
}
