using MyThings.Core.Entities;

using MyThings.Core.DTOs;
using MyThings.Core.Dto;

namespace MyThings.Core.Interfaces;

public interface IPartnerReadRepository : IReadOnlyRepository<Partner>
{
    Task<IReadOnlyList<Partner>> GetPartnersByDomainIdAsync(int domainId);
    Task<IReadOnlyList<Product>> GetProductsByPartnerId(int partnerId);
    Task<IReadOnlyList<OptionGroup>> GetProductOptionsByProductIdAsync(int productId);
    IQueryable<Partner> GetPartnersList();
    IQueryable<Partner> SearchPartnersByNameAndDescription(string searchTerm, int domainId);
    IQueryable<Partner> SearchPartnerByCategory(string searchTerm);
    IQueryable<Partner> SearchPartnerByProduct(string searchTerm);
    IQueryable<PartnerSearchResult> SearchPartners(string searchTerm, int domainId, double? userLat, double? userLon);
}