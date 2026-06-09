using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;


public class ReadUnitOfWork : IReadUnitOfWork
{
    private readonly ReadDbContext _context;

    public IPartnerReadRepository Partners {get;}
    public IReadOnlyRepository<Product> Products {get;}
    public IReadOnlyRepository<Domain> Domains {get;}
    public IReadOnlyRepository<Order> Orders {get;}
    public IReadOnlyRepository<Location> Locations {get;}
    public IReadOnlyRepository<Customer> Customers {get;}
    public IReadOnlyRepository<DeliveryRule> DeliveryRules {get;}
    public IReadOnlyRepository<ProductOption> ProductOptions {get;}
    public IReadOnlyRepository<Driver> Drivers {get;}

    public ReadUnitOfWork(ReadDbContext context)
    {
        _context = context;
        
        Partners = new PartnerReadRepository(_context);
        Products = new ReadOnlyRepository<Product>(_context);
        Domains = new ReadOnlyRepository<Domain>(_context);
        Orders = new ReadOnlyRepository<Order>(_context);
        Locations = new ReadOnlyRepository<Location>(_context);
        DeliveryRules = new ReadOnlyRepository<DeliveryRule>(_context);
        Customers = new ReadOnlyRepository<Customer>(_context);
        ProductOptions = new ReadOnlyRepository<ProductOption>(_context);
        Drivers = new ReadOnlyRepository<Driver>(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
    
}