using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client.RP;
namespace MyThings.Core.Entities;

public class Admin : User
{
    public string EmployeeId { get; set; } = null!;
    public int JobId {get;set;}
    public string Department { get; set; } = null!; 
    public string PasswordHash { get; set; } = null!;

    public virtual Job? Job {get;set;} 
    
    internal class AdminConfiguration : BaseEntityConfiguration<Admin>
    {
        public AdminConfiguration() : base(isDerived: true) { }
        public override void Configure(EntityTypeBuilder<Admin> builder)
        {
            base.Configure(builder);

            builder.ToTable("Admin");

            builder.Property(a => a.EmployeeId)
                .HasMaxLength(20)
                .IsRequired();

            builder.HasIndex(a => a.EmployeeId)
                .HasDatabaseName("IX_ADMIN_ID_UNIQUE")
                .IsUnique();

            builder.Property(a => a.Department)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasOne(a => a.Job)
                .WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);
            
        }
    }

}