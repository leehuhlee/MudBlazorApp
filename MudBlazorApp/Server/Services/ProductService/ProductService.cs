using MudBlazorApp.Server.Services.ExcelService;
using MudBlazorApp.Shared.Models;
using OfficeOpenXml;

namespace MudBlazorApp.Server.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IExcelService _excelService;

        public ProductService(DataContext context, IExcelService excelService)
        {
            _context = context;
            _excelService = excelService;
        }

        public async Task<ServiceResponse<List<Product>>> GetProductsAsync()
        {
            var result = await _context.Products.ToListAsync();
            return new ServiceResponse<List<Product>> { Data = result };
        }

        public async Task<ServiceResponse<bool>> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Create Product Successful!" };
        }

        public async Task<ServiceResponse<bool>> UpdateProductAsync(Product product)
        {
            var findProduct = await _context.Products.FindAsync(product.Id);
            findProduct.Name = product.Name;
            findProduct.Description = product.Description;
            findProduct.BrandId = product.BrandId;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Update Product Successful!" };
        }

        public async Task<ServiceResponse<bool>> DeleteProductAsync(int productId)
        {
            var findProduct = await _context.Products.FindAsync(productId);
            _context.Products.Remove(findProduct);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Data = true, Message = "Delete Product Successful!" };
        }

        public ExcelPackage ExportProductToExcel(List<ProductTable> productTableList)
        {
            var package = _excelService.CreateExcelPackage(productTableList,
                mappers: new Dictionary<string, Func<ProductTable, object>>
                {
                    { nameof(ProductTable.Id), item => item.Id },
                    { nameof(ProductTable.Name), item => item.Name },
                    { nameof(ProductTable.Description), item => item.Description },
                    { nameof(ProductTable.Brand), item => item.Brand }
                },
                sheet: nameof(Product));

            return package;
        }
    }
}
