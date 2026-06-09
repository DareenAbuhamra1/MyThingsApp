using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

// Has All repositories
public interface IUnitOfWork
{
    // Users
    IGenericRepository<User> Users {get;}
    IAdminRepository Admins { get; }
    IGenericRepository<Driver> Drivers {get;}
    IGenericRepository<Customer> Customers {get;}
    IGenericRepository<Partner> Partners {get;}

    // Business
    IGenericRepository<Order> Orders {get;}
    IGenericRepository<OrderLine> OrderLines {get;}
    IGenericRepository<OrderLineOption> OrderLineOptions {get;}
    IGenericRepository<OptionGroup> OptionGroups {get;}
    IGenericRepository<Product> Products {get;}
    IGenericRepository<ProductOption> ProductOptions {get;}
    IGenericRepository<PartnerCategory> PartnerCategories {get;}
    IGenericRepository<PartnerDomain> PartnerDomains {get;}
    IGenericRepository<WorkingHour> WorkingHours {get;}
    IGenericRepository<Location> Locations {get;}
    IGenericRepository<Domain> Domains {get;}
    IGenericRepository<DeliveryRule> DeliveryRules {get;}
    IGenericRepository<Category> Categories {get;}
    IGenericRepository<AuditLog> AuditLogs {get;}
    IGenericRepository<Job> Jobs {get;}

    Task<int> CompleteAsync();
}