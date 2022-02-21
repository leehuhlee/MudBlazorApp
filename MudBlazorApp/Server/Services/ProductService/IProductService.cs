using MudBlazorApp.Shared.Models;
using OfficeOpenXml;

namespace MudBlazorApp.Server.Services.ProductService
{
    public interface IProductService
    {
        Task<ServiceResponse<List<Product>>> GetProductsAsync();
        Task<ServiceResponse<bool>> CreateProductAsync(Product product);
        Task<ServiceResponse<bool>> UpdateProductAsync(Product product);
        Task<ServiceResponse<bool>> DeleteProductAsync(int productId);
        ExcelPackage ExportProductToExcel(List<ProductTable> productTableList);
    }
}
