using Microsoft.EntityFrameworkCore;

namespace MyThings.Core.Entities;


public abstract class BaseDbContext : DbContext
{
    protected BaseDbContext(DbContextOptions Options) :base(Options){}

    public virtual DbSet<Admin> Admins {get;set;}
    public virtual DbSet<AuditLog> AuditLogs {get;set;}
    public virtual DbSet<Category> Categories {get;set;}
    public virtual DbSet<Customer> Customers {get;set;}
    public virtual DbSet<DeliveryRule> DeliveryRules {get;set;}
    public virtual DbSet<Domain> Domains {get;set;}
    public virtual DbSet<Driver> Drivers {get;set;}
    public virtual DbSet<Job> Jobs {get;set;}
    public virtual DbSet<Location> Locations {get;set;}
    public virtual DbSet<OptionGroup> OptionGroups {get;set;}
    public virtual DbSet<Order> Orders {get;set;}
    public virtual DbSet<OrderLine> OrderLines {get;set;}
    public virtual DbSet<OrderLineOption> OrderLineOptions {get;set;}
    public virtual DbSet<Partner> Partners {get;set;}
    public virtual DbSet<PartnerCategory> PartnerCategories {get;set;}
    public virtual DbSet<PartnerDomain> PartnerDomains {get;set;}
    public virtual DbSet<Product> Products {get;set;}
    public virtual DbSet<ProductOption> ProductOptions {get;set;}
    public virtual DbSet<User> Users {get;set;}
    public virtual DbSet<WorkingHour> WorkingHours {get;set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BaseEntity).Assembly);
    }

}