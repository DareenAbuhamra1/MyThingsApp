using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyThings.Core.Entities;

public class AuditLog : BaseEntity
{
    public int AdminId {get;set;}
    public virtual Admin Admin {get;set;} = null!;
    public string Action { get; set; } = null!; // e.g., "UpdateFee", "ApprovePartner"
    public string EntityName { get; set; } = null!; // e.g., "Store", "Driver"
    public int EntityId { get; set; } // The ID of the affected store/driver

    internal class AuditLogConfiguration : BaseEntityConfiguration<AuditLog>
    {
        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            base.Configure(builder);

            builder.ToTable("AuditLog");

            builder.Property(a => a.Action)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.EntityName)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(a => a.Action);
            builder.HasIndex(a => a.EntityId);
            builder.HasIndex(a => a.AdminId);
            
            builder.HasOne(a => a.Admin)
                   .WithMany() 
                   .HasForeignKey(a => a.AdminId)
                   .OnDelete(DeleteBehavior.Restrict); // Don't delete logs if a user is deleted
        }
    }
}