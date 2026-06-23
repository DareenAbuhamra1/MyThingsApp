using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class CustomerStatus : BaseEntity
{
    public string Name { get; set; } = null!;
    
    public virtual ICollection<CustomerStatusTranslation> Translations { get; set; } = [];

    internal class CustomerStatusConfiguration : BaseEntityConfiguration<CustomerStatus>
    {
        public override void Configure(EntityTypeBuilder<CustomerStatus> builder)
        {
            base.Configure(builder);

            builder.ToTable("CustomerStatus");

            builder.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasMany(cs => cs.Translations)
                .WithOne(cst => cst.CustomerStatus)
                .HasForeignKey(cst => cst.CustomerStatusId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
