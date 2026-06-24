using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using MyThings.Core.DTOs;
using MyThings.Core.Dto;
using System.Security.Cryptography;
using MyThings.Infrastructure.Extensions;
namespace MyThings.Infrastructure.Repositories;

public class PartnerReadRepository : ReadOnlyRepository<Partner>, IPartnerReadRepository
{

    public PartnerReadRepository(ReadDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Partner>> GetPartnersByDomainIdAsync(int domainId)
    {
        return await _context.Partners
            .InDomain(domainId)
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

    public IQueryable<Partner> SearchPartnersByNameAndDescription(string searchTerm, int domainId)
    {
        return _context.Partners
            .IsValidRow()
            .InDomain(domainId)
            .SearchText(searchTerm)
            .Include(p => p.Location)
            .AsQueryable();
    }

    public IQueryable<Partner> SearchPartnerByCategory(string searchTerm)
    {
        return _context.Partners
            .IsValidRow()
            .SearchInCategories(searchTerm)
            .Include(p => p.Location)
            .AsQueryable();
    }

    public IQueryable<Partner> SearchPartnerByProduct(string searchTerm)
    {
        return _context.Partners
            .IsValidRow()
            .SearchInProducts(searchTerm)
            .Include(p => p.Location)
            .AsQueryable();
    }

    public IQueryable<PartnerSearchResult> SearchPartners(string searchTerm, int domainId, double? userLat, double? userLon)
    {
        var query = _context.Partners
            .IsValidRow()
            .InDomain(domainId)
            .FullSearch(searchTerm)
            .Include(pl => pl.Location);

        double userLatRadians = userLat.HasValue? userLat.Value * Math.PI / 180.0 : 0;
        double userLonRadians = userLon.HasValue? userLon.Value * Math.PI / 180.0 : 0;

        return query.Select(p => new PartnerSearchResult
        {
            Partner = p,
            Order = p.Name.Contains(searchTerm) 
                || p.DescriptionEn.Contains(searchTerm) 
                || p.DescriptionAr.Contains(searchTerm) ? 0:
                p.PartnerCategories.Any(pc => pc.Category != null
                && pc.Category.Name.Contains(searchTerm)) ? 1:3, // order = 3 like in the SP

            Distance = !userLat.HasValue || !userLon.HasValue ? 0:
                6371 * Math.Acos(
                    Math.Cos(userLatRadians) * Math.Cos( (double)p.Location.Latitude * Math.PI / 180.0)
                    * Math.Cos((double)p.Location.Longitude * Math.PI /180.0 - userLonRadians) +
                    Math.Sin(userLatRadians) *Math.Sin((double)p.Location.Latitude * Math.PI /180.0 )

                )
        });
    }
}