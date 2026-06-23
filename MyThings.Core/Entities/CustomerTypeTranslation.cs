using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class CustomerTypeTranslation : BaseEntity
{
    public int CustomerTypeId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; } = null!;

    public virtual Language? Language { get; set; }

    internal class CustomerTypeTranslationConfiguration : BaseEntityConfiguration<CustomerTypeTranslation>
    {
        public override void Configure(EntityTypeBuilder<CustomerTypeTranslation> builder)
        {
            base.Configure(builder);

            builder.ToTable("CustomerTypeTranslation");

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(ctt => ctt.Language)
                .WithMany(l => l.CustomerTypeTranslations)
                .HasForeignKey(ctt => ctt.LanguageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Composite unique constraint to ensure only one translation per type per language
            builder.HasIndex(ctt => new { ctt.CustomerTypeId, ctt.LanguageId })
                .IsUnique();
        }
    }
}
