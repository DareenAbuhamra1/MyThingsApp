using Microsoft.AspNetCore.Mvc;
using MyThings.Infrastructure.Helper;
using MyThings.Core.Enums;
using MyThings.Core.Interfaces;
using System.ServiceModel.Channels;

namespace ProductController.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger; 
        private readonly IProductService _productService;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductController(ILogger<ProductController> logger, IProductService productService, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _productService = productService;
            _scopeFactory = scopeFactory;
        }

        [Authorize(RoleEnum.Admin, RoleEnum.SuperAdmin)]
        [HttpPost("upload-menu/{partnerId}")]
        public async Task<IActionResult> UploadMenu([FromRoute] int partnerId,  IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded for partner with id {PartnerId}.", partnerId);
                return BadRequest(new { Message = "No file uploaded. Please select a file to upload." });
            }
            if(!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid file format uploaded for partner with id {PartnerId}. File name: {FileName}.", partnerId, file.FileName);
                return BadRequest(new { Message = "Invalid file format. Please upload an Excel file with .xlsx extension." });
            }
            try
            {
                var uploadResult = await _productService.UploadMenuFromExcelAsync(partnerId,file.OpenReadStream());
                if(uploadResult == 0)
                {
                    _logger.LogWarning("No products uploaded for partner with id {PartnerId}.", partnerId);
                    return BadRequest("The uploaded file either doesn't contain products or invalid data");
                }
                _logger.LogInformation("Menu uploaded successfully for partner with id {PartnerId}. Added {Count} products.", partnerId, uploadResult);
                return Ok(new { Message = "Menu uploaded successfully." , Count = uploadResult });

            }
            catch(IOException ie)
            {
                _logger.LogError(ie,"An error occured opening the file");
                return StatusCode(500, new {Message = "An error occurred while opening the file. Please try again later."});
            }
            catch(Exception e)
            {
                _logger.LogError(e, "An error occurred while uploading menu for partner with id {PartnerId}.", partnerId);
                return StatusCode(500, new { Message = "An internal error occurred while uploading the menu. Please try again later." });   
            }
        }
        [Authorize(RoleEnum.Admin,RoleEnum.SuperAdmin)]
        [HttpPost("upload-menu-fire-and-forget/{partnerId}")]
        public async Task<IActionResult> UploadMenuFireAndForget([FromRoute] int partnerId, IFormFile file)
        {
            if(file == null || file.Length == 0)
            {
                _logger.LogWarning("No file uploaded for partner with id {PartnerId}.", partnerId);
                return BadRequest(new { Message = "No file uploaded. Please select a file to upload." });
            }
            if(!file.FileName.EndsWith(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid file format uploaded for partner with id {PartnerId}. File name: {FileName}.", partnerId, file.FileName);
                return BadRequest(new { Message = "Invalid file format. Please upload an Excel file with .xlsx extension." });
            }
            try
            {
                var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);

                memoryStream.Position = 0;

                var parentActivity = System.Diagnostics.Activity.Current;
                var traceId = parentActivity?.TraceId.ToString() ?? HttpContext.TraceIdentifier;

                _logger.LogInformation("Initiating fire-and-forget menu import tracking payload for Partner {PartnerId}. Trace ID: {TraceId}", partnerId, traceId);

                _ = Task.Run(async () =>
                {
                    using var scope = _scopeFactory.CreateScope();

                    var scopedProductService = scope.ServiceProvider.GetRequiredService<IProductService>();
                    var scopedLogger = scope.ServiceProvider.GetRequiredService<ILogger<ProductController>>();  

                    try
                    {
                        scopedLogger.LogInformation("Background thread worker context activated for Partner {PartnerId}. Processing binary graph...", partnerId);

                        await scopedProductService.UploadMenuFromExcelAsync(partnerId,memoryStream);

                        scopedLogger.LogInformation("Background execution resolved successfully for Partner {PartnerId}.", partnerId);
                    }
                    catch(Exception e)
                    {
                        scopedLogger.LogError(e, "Background processing failed unexpectedly for Partner {PartnerId}.", partnerId);
                    }
                    finally
                    {
                        await memoryStream.DisposeAsync();
                    }
                });

                _logger.LogInformation("The product file is uploading");
                return Ok("The file is uploading");
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An error occured while uploading the products for partner with id {PartnerId}.", partnerId);
                return BadRequest(new {Message = "An internal error occurred while uploading the menu. Please try again later."});
            }
        }
    }
}