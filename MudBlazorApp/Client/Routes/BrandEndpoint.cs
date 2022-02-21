namespace MudBlazorApp.Client.Routes
{
    public static class BrandEndpoints
    {
        public static string GetBrands = "api/brand/brands";
        public static string CreateBrand = "api/brand/create";
        public static string UpdateBrand = "api/brand/update";
        public static string ExportBrand = "api/brand/export";
    
        public static string DeleteBrand(int brandId)
        {
            return $"api/brand/delete/{brandId}";
        }

        public static string GetBrand(int brandId)
        {
            return $"api/brand/brands/{brandId}";
        }
    }
}
