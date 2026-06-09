using System.Net.NetworkInformation;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Core.Enums.GenderEnum;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;

namespace MyThings.Core.Entities;

public abstract class User : BaseEntity, IActivatable
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public GenderEnum Gender { get; set; }
    public RoleEnum Role {get;set;}
    public bool IsActive { get; set; }
    public DateTime? LastLogin { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    internal class UserConfiguration : BaseEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("User");

            builder.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.Gender)
               .HasMaxLength(10)
               .HasDefaultValue(GenderEnum.Undefined);

            builder.Property(e => e.Role)
                .IsRequired();

            builder.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .IsRequired();

            builder.HasIndex(e => e.Phone)
                .HasDatabaseName("IX_USER_PHONE_UNIQUE")
                .IsUnique();

            builder.HasIndex(e => e.Email) // Adding this for faster lookups/uniqueness
                .IsUnique()
                .HasFilter("[Email] IS NOT NULL"); // Allows multiple nulls, but only one "ahmad@gmail.com"

            builder.Property(e => e.RefreshToken)
                .HasMaxLength(500)
                .IsUnicode(false);

            builder.Property(e => e.DateOfBirth)
                .HasColumnType("date");

            builder.Property(e => e.LastLogin)
                .HasColumnType("datetime2");

            builder.Property(e => e.RefreshTokenExpiry)
                .HasColumnType("datetime2");
        }
    }
}

