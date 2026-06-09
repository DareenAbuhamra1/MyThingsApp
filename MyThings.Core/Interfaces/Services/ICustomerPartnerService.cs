using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;


public interface ICustomerPartnerService
{
    Task<List<StoreDisplayDto>> GetPartnersAsync(int DomainId);
    Task<List<ProductDisplayDto>> GetProductsAsync(int StoreId);
    Task<List<ProductOptionDisplayDto>> GetProductOptionsAsync(int productId);

}