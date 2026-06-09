using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Data.Enums;


namespace MyThings.Core.Entities;

public class WorkingHour : BaseEntity
{
    public int? PartnerId { get; set; }

    public DayEnum Day { get; set; }

    public TimeOnly ShiftBegin { get; set; }

    public TimeOnly ShiftEnd { get; set; }

    public virtual Partner? Partner { get; set; } 

    internal class WorkingHoursConfiguration : BaseEntityConfiguration<WorkingHour>
    {
        public override void Configure(EntityTypeBuilder<WorkingHour> builder)
        {
            base.Configure(builder);

            builder.ToTable("WorkingHour");

            builder.Property(e => e.Day)
                .IsRequired();

            builder.Property(e => e.ShiftBegin)
                .IsRequired();

            builder.Property(e => e.ShiftEnd)
                .IsRequired();
        }
    }
}