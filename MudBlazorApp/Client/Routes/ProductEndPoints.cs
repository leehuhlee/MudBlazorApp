namespace MudBlazorApp.Client.Routes
{
    public static class ProductEndPoints
    {
        public static string GetProducts = "api/product/products";
        public static string CreateProduct = "api/product/create";
        public static string UpdateProduct = "api/product/update";
        public static string ExportProduct = "api/product/export";

        public static string DeleteProduct(int productId)
        {
            return $"api/product/delete/{productId}";
        }
    }
}
