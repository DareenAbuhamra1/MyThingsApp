using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Hybrid;
using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Mappers;

namespace MyThings.Infrastructure.Services;

public class CustomerPartnerService : ICustomerPartnerService
{
    private readonly IPartnerReadRepository _partnerRepository;
    private readonly ProductOptionDisplayMapper _productOptionDisplayMapper;
    private readonly ProductDisplayMapper _productDisplayMapper;
    private readonly StoreDisplayMapper _storeDisplayMapper;
    private readonly HybridCache _hybridCache;

    public CustomerPartnerService(IPartnerReadRepository partnerRepository, ProductOptionDisplayMapper productOptionDisplayMapper, ProductDisplayMapper productDisplayMapper, StoreDisplayMapper storeDisplayMapper, HybridCache hybridCache
    )
    {
        _partnerRepository = partnerRepository;
        _productOptionDisplayMapper = productOptionDisplayMapper;
        _productDisplayMapper = productDisplayMapper;
        _storeDisplayMapper = storeDisplayMapper;
        _hybridCache = hybridCache;
    }

    public async Task<List<StoreDisplayDto>> GetPartnersAsync(int domainId)
    {
        return await _hybridCache.GetOrCreateAsync(
            $"Partners:{domainId}",
            async ct =>
            {
                var partners = await _partnerRepository.GetPartnersByDomainIdAsync(domainId);

                return partners.Select(p => _storeDisplayMapper.Map(p)).ToList();
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
            }
        );
    }
    public async Task<List<ProductDisplayDto>> GetProductsAsync(int partnerId)
    {

        return await _hybridCache.GetOrCreateAsync(
            $"Products:{partnerId}",
            async ct =>
            {
                var products = await _partnerRepository.GetProductsByPartnerId(partnerId);
                return products.Select(p => _productDisplayMapper.Map(p)).ToList();
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
            }
        );
    }
    public async Task<List<ProductOptionDisplayDto>> GetProductOptionsAsync(int productId)
    {
        return await _hybridCache.GetOrCreateAsync(
            $"ProductOptions:{productId}",
            async ct =>
            {
                var productOptions = await _partnerRepository.GetProductOptionsByProductIdAsync(productId);

                return productOptions.Select(og => _productOptionDisplayMapper.Map(og)).ToList();
            },
            new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
            }
        );
    }
}