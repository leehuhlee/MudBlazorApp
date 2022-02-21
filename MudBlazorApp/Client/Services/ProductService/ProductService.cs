using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;

        public ProductService(HttpClient http)
        {
            _http = http;
        }

        public async Task<int> GetProductCountAsync()
        {
            var productList = await GetProductsAsync();
            return productList.Count;
        }

        public async Task<double[]> GetProductMonthlyCountAsync()
        {
            var productList = await GetProductsAsync();
            double[] ProductMonthlyCount = new double[12];
            foreach (var product in productList)
                ProductMonthlyCount[product.CreatedDate.Month - 1]++;
            return ProductMonthlyCount;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<List<Product>>>(ProductEndPoints.GetProducts);
            if (result.Success)
                return result.Data;
            return new List<Product>();
        }

        public async Task<ServiceResponse<bool>> CreateProductAsync(Product product)
        {
            var result = await _http.PostAsJsonAsync(ProductEndPoints.CreateProduct, product);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> UpdateProductAsync(Product product)
        {
            var result = await _http.PutAsJsonAsync(ProductEndPoints.UpdateProduct, product);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int productId)
        {
            var result = await _http.DeleteAsync(ProductEndPoints.DeleteProduct(productId));
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<Stream> ExportProductAsync(List<ProductTable> productTableList)
        {
            var response = await _http.PostAsJsonAsync(ProductEndPoints.ExportProduct, productTableList);
            response.EnsureSuccessStatusCode();
            var fileBytes = await response.Content.ReadAsStreamAsync();
            return fileBytes;
        }
    }
}
