using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyThings.Core.Enums;
namespace MyThings.Core.Entities;
public class Order : BaseEntity
{
    public OrderStatusEnum Status {get;set;}
    public int CustomerId {get;set;}
    public virtual Customer Customer {get;set;} = null!; 
    public decimal SubTotal {get;set;}
    public decimal ServiceFee {get;set;}
    public decimal DeliveryFees {get;set;}
    public decimal? SavingAmount {get;set;}
    public decimal TotalPayment {get;set;}
    public OrderPaymentTypeEnum? PaymentType {get;set;}
    public TimeOnly? StartEstimation {get;set;}
    public TimeOnly? EndEstimation {get;set;}
    public DateTime? PlacementTime {get;set;}
    public DateTime? AcceptedTime {get;set;}
    public DateTime? PickedUpTime {get;set;}
    public DateTime? DeliveredTime {get;set;}
    public int? DriverId {get;set;}
    public virtual Driver? Driver {get;set;}
    public int PartnerId {get;set;} 
    public virtual Partner Partner {get;set;} = null!;
    public int DomainId {get;set;}
    public virtual Domain Domain {get;set;} = null!;
    public int DeliveryRuleId {get;set;} 
    public virtual DeliveryRule DeliveryRule {get;set;} = null!;
    public int DeliveryLocationId {get;set;}
    public virtual Location DeliveryLocation {get;set;} = null!;
    public string? Note {get;set;}
    
    public virtual ICollection<OrderLine> OrderLines {get;set;} = [];

    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Order");

            builder.Property(o => o.TotalPayment).HasColumnType("decimal(18,3)");
            builder.Property(o => o.SubTotal).HasColumnType("decimal(18,3)");
            builder.Property(o => o.ServiceFee).HasColumnType("decimal(18,3)");
            builder.Property(o => o.DeliveryFees).HasColumnType("decimal(18,3)");
            builder.Property(o => o.SavingAmount).HasColumnType("decimal(18,3)");
            builder.Property(o => o.PlacementTime);
            builder.Property(o => o.AcceptedTime);
            builder.Property(o => o.PickedUpTime);
            builder.Property(o => o.DeliveredTime);

            builder.HasOne(o => o.Customer)
                   .WithMany(c =>c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Restrict);
           
            builder.HasOne(o => o.Driver)
                   .WithMany(d => d.Orders)
                   .HasForeignKey(o => o.DriverId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.Partner)
                   .WithMany()
                   .HasForeignKey(o => o.PartnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.DeliveryRule)
                .WithMany()
                .HasForeignKey(o => o.DeliveryRuleId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}