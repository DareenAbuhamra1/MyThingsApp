using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly WriteDbContext _context;

    // Users
    public IGenericRepository<User> Users {get;private set;}
    public IAdminRepository Admins {get;private set;}
    public IGenericRepository<Driver> Drivers {get;private set;}
    public IGenericRepository<Customer> Customers {get;private set;}
    public IGenericRepository<Partner> Partners {get;private set;}
    

    // Business
    public IGenericRepository<Order> Orders {get;private set;}
    public IGenericRepository<OrderLine> OrderLines {get;private set;}
    public IGenericRepository<OrderLineOption> OrderLineOptions {get;private set;}
    public IGenericRepository<OptionGroup> OptionGroups {get;private set;}
    public IGenericRepository<Product> Products {get;private set;}
    public IGenericRepository<ProductOption> ProductOptions {get;private set;}
    public IGenericRepository<PartnerCategory> PartnerCategories {get;private set;}
    public IGenericRepository<PartnerDomain> PartnerDomains {get;private set;}
    public IGenericRepository<WorkingHour> WorkingHours {get;private set;}
    public IGenericRepository<Location> Locations {get;private set;}
    public IGenericRepository<Domain> Domains {get;private set;}
    public IGenericRepository<DeliveryRule> DeliveryRules {get;private set;}
    public IGenericRepository<Category> Categories {get;private set;}
    public IGenericRepository<AuditLog> AuditLogs {get;private set;}
    public IGenericRepository<Job> Jobs {get;private set;}
    public UnitOfWork(WriteDbContext context)
    {
        _context = context;
        
        // Identity & Roles
        Users = new GenericRepository<User>(_context);
        Admins = new AdminRepository(_context);
        Drivers = new GenericRepository<Driver>(_context);
        Customers = new GenericRepository<Customer>(_context);
        Partners = new GenericRepository<Partner>(_context);

        // Orders & Transactions
        Orders = new GenericRepository<Order>(_context);
        OrderLines = new GenericRepository<OrderLine>(_context);
        OrderLineOptions = new GenericRepository<OrderLineOption>(_context);

        // Products & Catalog
        Products = new GenericRepository<Product>(_context);
        ProductOptions = new GenericRepository<ProductOption>(_context);
        OptionGroups = new GenericRepository<OptionGroup>(_context);
        Categories = new GenericRepository<Category>(_context);

        // Partner Metadata
        PartnerCategories = new GenericRepository<PartnerCategory>(_context);
        PartnerDomains = new GenericRepository<PartnerDomain>(_context);
        WorkingHours = new GenericRepository<WorkingHour>(_context);
        
        // Infrastructure & Rules
        Locations = new GenericRepository<Location>(_context);
        Domains = new GenericRepository<Domain>(_context);
        DeliveryRules = new GenericRepository<DeliveryRule>(_context);
        
        // System
        AuditLogs = new GenericRepository<AuditLog>(_context);

        //Admin Jobs
        Jobs = new GenericRepository<Job>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        return await _context.SaveChangesAsync();
    }

    // Standard practice to release database resources
    public void Dispose()
    {
        _context.Dispose();
    }

}