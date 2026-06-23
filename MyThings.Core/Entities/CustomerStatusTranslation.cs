using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class CustomerStatusTranslation : BaseEntity
{
    public int CustomerStatusId { get; set; }
    public int LanguageId { get; set; }
    public string Name { get; set; } = null!;

    public virtual CustomerStatus? CustomerStatus { get; set; }
    public virtual Language? Language { get; set; }

    internal class CustomerStatusTranslationConfiguration : BaseEntityConfiguration<CustomerStatusTranslation>
    {
        public override void Configure(EntityTypeBuilder<CustomerStatusTranslation> builder)
        {
            base.Configure(builder);

            builder.ToTable("CustomerStatusTranslation");

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasOne(cst => cst.CustomerStatus)
                .WithMany(cs => cs.Translations)
                .HasForeignKey(cst => cst.CustomerStatusId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cst => cst.Language)
                .WithMany(l => l.CustomerStatusTranslations)
                .HasForeignKey(cst => cst.LanguageId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            // Composite unique constraint to ensure only one translation per status per language
            builder.HasIndex(cst => new { cst.CustomerStatusId, cst.LanguageId })
                .IsUnique();
        }
    }
}
