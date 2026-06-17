using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Dto;

namespace MyThings.Infrastructure.Repositories;

public class PartnerReadRepository : ReadOnlyRepository<Partner>, IPartnerReadRepository
{

    public PartnerReadRepository(ReadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Partner>> GetPartnersByDomainIdAsync(int domainId)
    {
        return await _context.Partners
            .Where(p => p.PartnerDomains
            .Any(d => d.DomainId == domainId))
            .ToListAsync();
    }
    public async Task<IReadOnlyList<Product>> GetProductsByPartnerId(int partnerId)
    {
        return await _context.Products
            .Where(p => p.PartnerId == partnerId)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<OptionGroup>> GetProductOptionsByProductIdAsync(int productId)
    {
        return await _context.OptionGroups
            .Include(og => og.ProductOptions)
            .Where(og => og.ProductId == productId)
            .ToListAsync();
    }

    public IQueryable<Partner> GetPartnersList()
    {
        return _context.Partners
            .Include(p => p.Location);
    }
}