using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
    private readonly RedisCacheService _cache; 
    private readonly HybridCacheService _hybridCache;

    public CustomerPartnerService(IPartnerReadRepository partnerRepository, ProductOptionDisplayMapper productOptionDisplayMapper, ProductDisplayMapper productDisplayMapper, StoreDisplayMapper storeDisplayMapper, RedisCacheService cache,
        HybridCacheService hybridCache
    )
    {
        _partnerRepository = partnerRepository;
        _productOptionDisplayMapper = productOptionDisplayMapper;
        _productDisplayMapper = productDisplayMapper;
        _storeDisplayMapper = storeDisplayMapper;
        _cache = cache;
        _hybridCache = hybridCache;
    }

    public async Task<List<StoreDisplayDto>> GetPartnersAsync(int domainId)
    {
        var partnersList = await _hybridCache.GetOrCreateAsync(
            $"partners:{domainId}",
            async () => {
                var partners = await _partnerRepository.GetPartnersByDomainIdAsync(domainId);

                return partners.Select(p => _storeDisplayMapper.Map(p)).ToList();
            },
            TimeSpan.FromMinutes(2), 
            TimeSpan.FromMinutes(30)
        );
        
        /*
        var partners = await _partnerRepository.GetPartnersByDomainIdAsync(domainId);

        var partnersList = partners.Select(p => _storeDisplayMapper.Map(p)).ToList();
        */

        return partnersList??[];
    }
    public async Task<List<ProductDisplayDto>> GetProductsAsync(int partnerId)
    {
        var cacheKey = $"Products:{partnerId}";
        var cached = await  _cache.GetAsync<List<ProductDisplayDto>>(cacheKey);

        if(cached is not null)
        {
            Console.WriteLine("Cache Hit: Returning Cached Products");
            return cached;
        }

        Console.WriteLine("Cache Miss: Returning Products from DB");
        var products = await _partnerRepository.GetProductsByPartnerId(partnerId);

        var productsList = products.Select(p => _productDisplayMapper.Map(p)).ToList();

        await _cache.SetAsync(cacheKey, productsList,TimeSpan.FromMinutes(30));

        return productsList;
    }
    public async Task<List<ProductOptionDisplayDto>> GetProductOptionsAsync(int productId)
    {
        var cacheKey = $"ProductOptions:{productId}";
        var cached = await _cache.GetAsync<List<ProductOptionDisplayDto>>(cacheKey);

        if(cached is not null)
        {
            Console.WriteLine("Cache Hit: Returning Cached Product Options");
            return cached;
        }
        Console.WriteLine("Cache Miss: Returning Product Options from DB");
        var productOptions = await _partnerRepository.GetProductOptionsByProductIdAsync(productId);

        var productOptionsList =  productOptions.Select( og => _productOptionDisplayMapper.Map(og)).ToList();

        await _cache.SetAsync(cacheKey, productOptionsList, TimeSpan.FromMinutes(30));

        return productOptionsList;
    }
}