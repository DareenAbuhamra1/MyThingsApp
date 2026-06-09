using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Core.Interfaces;
namespace MyThings.Core.Entities;

public abstract class BaseEntity : ISoftDeletable
{
    public int Id {get;set;}
    public DateTime CreatedAt {get;set;}
    public DateTime? UpdatedAt {get;set;}
    public DateTime? DeletedAt {get;set;}
    public bool IsDeleted {get;set;}
}

public abstract class BaseEntityConfiguration<TBase> : IEntityTypeConfiguration<TBase> 
    where TBase : BaseEntity
{
    private readonly bool _isDerived;

    protected BaseEntityConfiguration(bool isDerived = false)
    {
        _isDerived = isDerived;
    }
    public virtual void Configure(EntityTypeBuilder<TBase> builder)
    {
        if (!_isDerived)
        {
            builder.HasKey(e => e.Id);
        }

        builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

        builder.Property(e => e.CreatedAt)
               .IsRequired()
               .HasDefaultValueSql("GETUTCDATE()"); // Uses SQL Server's UTC time
        
        builder.Property(e => e.IsDeleted)
            .HasDefaultValue(false);
    }
}