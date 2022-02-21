using MudBlazorApp.Server.Services.ExcelService;
using MudBlazorApp.Shared.Models;
using OfficeOpenXml;

namespace MudBlazorApp.Server.Services.BrandService
{
    public class BrandService : IBrandService
    {
        private readonly DataContext _context;
        private readonly IExcelService _excelService;

        public BrandService(DataContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<ServiceResponse<Brand>> GetBrandAsync(int brandId)
        {
            var result = await _context.Brands.FindAsync(brandId);
            return new ServiceResponse<Brand> { Data = result };
        }

        public async Task<ServiceResponse<List<Brand>>> GetBrandsAsync()
        {
            var result = await _context.Brands.ToListAsync();
            return new ServiceResponse<List<Brand>> { Data = result };
        }

        public async Task<ServiceResponse<bool>> CreateBrandAsync(Brand brand)
        {
            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Message = "Create Brand Successful!",
                Data = true
            };
        }

        public async Task<ServiceResponse<bool>> UpdateBrandAsync(Brand brand)
        {
            var findBrand = await _context.Brands.FindAsync(brand.Id);
            findBrand.Name = brand.Name;
            findBrand.Description = brand.Description;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Message = "Update Brand Successful!",
                Data = true
            };
        }
        public async Task<ServiceResponse<bool>> DeleteBrandAsync(int brandId)
        {
            var findBrand = await _context.Brands.FindAsync(brandId);
            _context.Brands.Remove(findBrand);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool>
            {
                Message = "Delete Brand Successful!",
                Data = true
            };
        }

        public ExcelPackage ExportBrandToExcel(List<Brand> brandList)
        {
            var package = _excelService.CreateExcelPackage(brandList,
                mappers: new Dictionary<string, Func<Brand, object>>
                {
                    { nameof(Brand.Id), item => item.Id },
                    { nameof(Brand.Name), item => item.Name },
                    { nameof(Brand.Description), item => item.Description }
                },
                sheet: nameof(Brand));

            return package;
        }
    }
}
