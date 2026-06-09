using MyThings.Core.DTOs;
using MyThings.Core.Interfaces;

namespace MyThings.Infrastructure.Services;

public class CustomerPartnerService : ICustomerPartnerService
{
    private readonly IPartnerReadRepository _partnerRepository;

    public CustomerPartnerService(IPartnerReadRepository partnerRepository)
    {
        _partnerRepository = partnerRepository;
    }

    public async Task<List<StoreDisplayDto>> GetPartnersAsync(int DomainId)
    {
        var partners = await _partnerRepository.GetPartnersByDomainIdAsync(DomainId);

        var partnersList = new List<StoreDisplayDto>();

        foreach (var p in partners)
        {
            partnersList.Add(
                new StoreDisplayDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsAvailable = p.IsAvailable,
                }
            );
        }
        return partnersList;
    }
    public async Task<List<ProductDisplayDto>> GetProductsAsync(int partnerId)
    {
        var products = await _partnerRepository.GetProductsByPartnerId(partnerId);

        var productsList = new List<ProductDisplayDto>();

        foreach (var product in products)
        {
            productsList.Add(
                new ProductDisplayDto
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    Description = product.Description??"",
                }
            );
        }

        return productsList;
    }
    public async Task<List<ProductOptionDisplayDto>> GetProductOptionsAsync(int productId)
    {
        var productOptions = await _partnerRepository.GetProductOptionsByProductIdAsync(productId);

        return productOptions.Select(
            og => new ProductOptionDisplayDto
            {
                OptionGroupId = og.Id,
                ProductId = og.ProductId,
                Title = og.Title,
                IsRequired = og.IsRequired,
                MinSelection = og.MinSelection,
                MaxSelection = og.MaxSelection,
                Options = (og.ProductOptions ?? [])
                    .Select(po => new ProductOption
                    {
                        ProductOptionId = po.Id,
                        Option = po.Option,
                        Price = po.Price,
                    }).ToList()
            }
        ).ToList();
    }
}