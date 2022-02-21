using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.BrandService
{
    public interface IBrandService
    {
        Task<Brand> GetBrandAsync(int brandId);
        Task<List<Brand>> GetBrandsAsync();
        Task<int> GetBrandCountAsync();
        Task<double[]> GetBrandMonthlyCountAsync();
        Task<ServiceResponse<bool>> CreateBrandAsync(Brand brand);
        Task<ServiceResponse<bool>> UpdateBrandAsync(Brand brand);
        Task<ServiceResponse<bool>> DeleteBrandAsync(int brandId);
        Task<Stream> ExportBrandAsync(List<Brand> brandList);
    }
}
