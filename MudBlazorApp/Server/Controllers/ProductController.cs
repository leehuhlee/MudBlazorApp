using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Server.Services.ProductService;
using MudBlazorApp.Shared.Models;
using System.Globalization;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheet.sheet";

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("products")]
        public async Task<ActionResult<ServiceResponse<List<Product>>>> GetProductsAsync()
        {
            var response = await _productService.GetProductsAsync();
            if(!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateProductAsync(Product product)
        {
            var response = await _productService.CreateProductAsync(product);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateProductAsync(Product product)
        {
            var response = await _productService.UpdateProductAsync(product);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("delete/{productId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteProductAsync(int productId)
        {
            var response = await _productService.DeleteProductAsync(productId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("export")]
        public async Task<ActionResult<ServiceResponse<FileContentResult>>> ExportProductAsync(List<ProductTable> productTableList)
        {
            byte[] reportBytes;
            using (var package = _productService.ExportProductToExcel(productTableList))
            {
                reportBytes = package.GetAsByteArray();
            }
            return File(reportBytes, XlsxContentType, $"MyReport{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.xlsx");
        }
    }
}
