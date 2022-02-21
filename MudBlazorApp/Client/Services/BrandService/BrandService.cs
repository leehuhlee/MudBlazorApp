using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;
using System.Globalization;

namespace MudBlazorApp.Client.Services.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly HttpClient _http;

        public BrandService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Brand> GetBrandAsync(int brandId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Brand>>(BrandEndpoints.GetBrand(brandId));
            if (response != null && response.Data != null)
                return response.Data;
            return new Brand();
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Brand>>>(BrandEndpoints.GetBrands);
            if (response != null && response.Data != null)
                return response.Data;
            return new List<Brand>();
        }

        public async Task<int> GetBrandCountAsync()
        {
            var brandList = await GetBrandsAsync();
            return brandList.Count;
        }

        public async Task<double[]> GetBrandMonthlyCountAsync()
        {
            var brandList = await GetBrandsAsync();
            double[] BrandMonthlyCount = new double[12];
            foreach (var brand in brandList)
                BrandMonthlyCount[brand.CreatedDate.Month - 1]++;
            return BrandMonthlyCount;
        }

        public async Task<ServiceResponse<bool>> CreateBrandAsync(Brand brand)
        {
            var result = await _http.PostAsJsonAsync(BrandEndpoints.CreateBrand, brand);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> UpdateBrandAsync(Brand brand)
        {
            var result = await _http.PutAsJsonAsync(BrandEndpoints.UpdateBrand, brand);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> DeleteBrandAsync(int brandId)
        {
            var result = await _http.DeleteAsync(BrandEndpoints.DeleteBrand(brandId));
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<Stream> ExportBrandAsync(List<Brand> brandList)
        {
            var response = await _http.PostAsJsonAsync(BrandEndpoints.ExportBrand, brandList);
            response.EnsureSuccessStatusCode();
            var fileBytes = await response.Content.ReadAsStreamAsync();
            return fileBytes;
        }
    }
}
