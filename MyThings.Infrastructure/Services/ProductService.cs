using Microsoft.Extensions.Logging;
using MyThings.Core.DTOs;
using MyThings.Core.Entities;
using MyThings.Core.Interfaces;
using MyThings.Infrastructure.Context;
using OfficeOpenXml;

namespace MyThings.Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IUnitOfWork _unitOfWork;

  

    public ProductService(ILogger<ProductService> logger, IUnitOfWork unitOfWork,IReadUnitOfWork readUnitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<int> UploadMenuFromExcelAsync(int partnerId, Stream fileStream)
    {
        ExcelPackage.License.SetNonCommercialPersonal("DareenAbuhamra");

        var productsToAdd = new List<Product>();
        var importDate = DateTime.UtcNow;

        using (ExcelPackage excel = new ExcelPackage(fileStream))
        {
            var worksheet = excel.Workbook.Worksheets[0];
            var rowCount = worksheet.Dimension.Rows;

            for (int row = 2; row <= rowCount; row++)
            {
                var productName = worksheet.Cells[row, 1].Value?.ToString();
                var productPrice = worksheet.Cells[row, 2].Value?.ToString();
                var productStock = worksheet.Cells[row, 3].Value?.ToString();
                var productDescription = worksheet.Cells[row, 4].Value?.ToString();

                if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(productPrice) || string.IsNullOrEmpty(productStock)) continue;

                var newProduct = new Product
                {
                    PartnerId = partnerId,
                    Name = productName,
                    Price = decimal.Parse(productPrice),
                    Stock = int.Parse(productStock),
                    Description = productDescription,
                    CreatedAt = importDate,
                };

                productsToAdd.Add(newProduct);
            }
            if (productsToAdd.Any())
            {
                await _unitOfWork.Products.AddRangeAsync(productsToAdd);
                await _unitOfWork.CompleteAsync();
                return productsToAdd.Count();
            }
            else
            {
                return 0;
            }
        }
    }
    public async Task<bool> UpdateProductStockAsync(int partnerId, int productId, int stock)
    {
        var partner = await _unitOfWork.Partners.GetByIdAsync(partnerId);

        if (partner == null)
        {
            return false;
        }

        var product = await _unitOfWork.Products.GetByIdAsync(productId);

        if (product == null || product.PartnerId != partner.Id)
        {
            _logger.LogWarning("Unauthorized stock update attempt or product not found. Partner: {PartnerId}, Product: {ProductId}", partnerId, productId);
            return false;
        }

        product.Stock = stock;

        _unitOfWork.Products.Update(product);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}