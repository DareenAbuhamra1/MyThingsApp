using MyThings.Core.Entities;

namespace MyThings.Core.Interfaces;

public interface IReadUnitOfWork 
{
    IPartnerReadRepository Partners {get;}
    IReadOnlyRepository<Product> Products {get;}
    IReadOnlyRepository<Domain> Domains {get;}
    IReadOnlyRepository<Order> Orders {get;}
    IReadOnlyRepository<Location> Locations {get;}
    IReadOnlyRepository<DeliveryRule> DeliveryRules {get;}
    IReadOnlyRepository<Customer> Customers {get;}
    IReadOnlyRepository<ProductOption> ProductOptions {get;}
    IReadOnlyRepository<Driver> Drivers {get;}
    void Dispose();

}

