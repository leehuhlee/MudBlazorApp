using MudBlazorApp.Shared.Models;
using OfficeOpenXml;

namespace MudBlazorApp.Server.Services.BrandService
{
    public interface IBrandService
    {
        Task<ServiceResponse<Brand>> GetBrandAsync(int brandId);
        Task<ServiceResponse<List<Brand>>> GetBrandsAsync();
        Task<ServiceResponse<bool>> CreateBrandAsync(Brand brand);
        Task<ServiceResponse<bool>> UpdateBrandAsync(Brand brand);
        Task<ServiceResponse<bool>> DeleteBrandAsync(int brandId);
        ExcelPackage ExportBrandToExcel(List<Brand> brandList);
    }
}
