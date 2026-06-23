using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class Language : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public bool IsActive { get; set; }
    
    public virtual ICollection<CustomerStatusTranslation> CustomerStatusTranslations { get; set; } = [];
    public virtual ICollection<CustomerTypeTranslation> CustomerTypeTranslations { get; set; } = [];

    internal class LanguageConfiguration : BaseEntityConfiguration<Language>
    {
        public override void Configure(EntityTypeBuilder<Language> builder)
        {
            base.Configure(builder);

            builder.ToTable("Language");

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.Code)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.HasIndex(e => e.Code)
                .IsUnique();
        }
    }
}
