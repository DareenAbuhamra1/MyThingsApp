using Microsoft.AspNetCore.Http;
using MyThings.Core.DTOs;

namespace MyThings.Core.Interfaces;

public interface IProductService
{
    Task<int> UploadMenuFromExcelAsync(int partnerId, Stream fileStream);
    Task<bool> UpdateProductStockAsync(int partnerId, int productId, int stock);
}

