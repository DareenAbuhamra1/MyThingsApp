using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;

namespace MyThings.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ReadDbContext _context;

    public ProductRepository(ReadDbContext context)
    {
        _context = context;
    }
}