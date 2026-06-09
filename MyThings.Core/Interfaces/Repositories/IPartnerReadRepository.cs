using MyThings.Core.Entities;

using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;

public interface IPartnerReadRepository : IReadOnlyRepository<Partner>
{
    Task<IReadOnlyList<Partner>> GetPartnersByDomainIdAsync(int domainId);
    Task<IReadOnlyList<Product>> GetProductsByPartnerId(int partnerId);
    Task<IReadOnlyList<OptionGroup>> GetProductOptionsByProductIdAsync(int productId);
    
}