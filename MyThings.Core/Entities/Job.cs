using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MyThings.Core.Entities;

public class Job : BaseEntity
{
    public string Title {get;set;} = null!;
    public bool CanManageAccounts {get;set;}
    public bool CanManageLogistics {get;set;}
    public bool CanManageProducts {get;set;}

    internal class JobConfiguration : BaseEntityConfiguration<Job>
    {
        public override void Configure(EntityTypeBuilder<Job> builder)
        {
            base.Configure(builder);

            builder.ToTable("Job");

            builder.Property(e => e.Title)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(e => e.CanManageAccounts)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.CanManageLogistics)
                .HasDefaultValue(false)
                .IsRequired();

            builder.Property(e => e.CanManageProducts)
                .HasDefaultValue(false)
                .IsRequired();
        }
    }
}