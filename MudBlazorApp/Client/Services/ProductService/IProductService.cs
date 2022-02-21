using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<int> GetProductCountAsync();
        Task<double[]> GetProductMonthlyCountAsync();
        Task<ServiceResponse<bool>> CreateProductAsync(Product product);
        Task<ServiceResponse<bool>> UpdateProductAsync(Product product);
        Task<ServiceResponse<bool>> DeleteProductAsync(int productId);
        Task<Stream> ExportProductAsync(List<ProductTable> productTableList);
    }
}
