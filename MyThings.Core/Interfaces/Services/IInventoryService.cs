using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;

public interface IInventoryService
{
    Task<ProductDto?> CreateProductAsync(ProductCreationDto creationDto);
    Task<ProductDto?> UpdateProductAsync(ProductUpdateDto updateDto);
    Task<bool> DeleteProductAsync(int id);
    /*
    // --- Bonus: Essential Inventory Logic ---
    // Since this is for 'Data Entry', you'll likely need these soon:
    Task<bool> UpdateStockLevelAsync(int productId, int newQuantity);
    Task<IEnumerable<ProductDto>> GetLowStockItemsAsync(int threshold);
    */
}