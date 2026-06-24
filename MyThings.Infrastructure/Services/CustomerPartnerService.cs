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

    public CustomerPartnerService(IPartnerReadRepository partnerRepository, ProductOptionDisplayMapper productOptionDisplayMapper, ProductDisplayMapper productDisplayMapper, StoreDisplayMapper storeDisplayMapper)
    {
        _partnerRepository = partnerRepository;
        _productOptionDisplayMapper = productOptionDisplayMapper;
        _productDisplayMapper = productDisplayMapper;
        _storeDisplayMapper = storeDisplayMapper;
    }

    public async Task<List<StoreDisplayDto>> GetPartnersAsync(int DomainId)
    {
        var partners = await _partnerRepository.GetPartnersByDomainIdAsync(DomainId);

        var partnersList = partners.Select(p => _storeDisplayMapper.Map(p)).ToList();
        return partnersList;
    }
    public async Task<List<ProductDisplayDto>> GetProductsAsync(int partnerId)
    {
        var products = await _partnerRepository.GetProductsByPartnerId(partnerId);

        var productsList = products.Select(p => _productDisplayMapper.Map(p)).ToList();

        return productsList;
    }
    public async Task<List<ProductOptionDisplayDto>> GetProductOptionsAsync(int productId)
    {
        var productOptions = await _partnerRepository.GetProductOptionsByProductIdAsync(productId);

        return productOptions.Select(
            og => _productOptionDisplayMapper.Map(og)
        ).ToList();
    }
}