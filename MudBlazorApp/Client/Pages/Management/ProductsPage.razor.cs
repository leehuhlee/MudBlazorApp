using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Models;
using System.Globalization;

namespace MudBlazorApp.Client.Pages.Management
{
    public partial class ProductsPage
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<Product> productList = new List<Product>();

        private List<ProductTable> productTableList = new List<ProductTable>();
        private List<ProductTable> searchProductTableData = new List<ProductTable>();
        private Product product;

        private string SearchString;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await Init();
            _loaded = true;
            HubConnection = HubConnection.TryInitialize(NavigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task Init()
        {
            productTableList.Clear();
            productList = await ProductService.GetProductsAsync();
            foreach(var product in productList)
            {
                var brandName = (await BrandService.GetBrandAsync(product.BrandId)).Name;
                productTableList.Add(new ProductTable()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Brand = brandName
                });
            }
            searchProductTableData = productTableList;
        }

        protected void FilterProduct(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                OnSearch();
            }
            else
            {
                productTableList = searchProductTableData;
            }
        }

        protected void OnSearch()
        {
            productTableList = searchProductTableData
                .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .ToList();
        }

        public void ResetSearch()
        {
            SearchString = string.Empty;
            productTableList = searchProductTableData;
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                product = productList.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    parameters.Add(nameof(AddEditProductModal.Product), new Product
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        BrandId = product.BrandId
                    });
                }
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<AddEditProductModal>(id == 0 ? "Create" : "Edit", parameters, options);
            await dialog.Result;

            await Init();
        }

        private async Task Delete(int brandId)
        {
            string deleteContent = "Delete Product";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, brandId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var response = await ProductService.DeleteProductAsync(brandId);
                SnackBar.Add(response.Message, Severity.Success);
                await Init();
            }
        }

        private async Task ExportToExcel()
        {
            var fileBytes = await ProductService.ExportProductAsync(productTableList);

            using (MemoryStream ms = new MemoryStream())
            {
                fileBytes.CopyTo(ms);
                var fileName = $"ProductList{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.xlsx";
                await JSRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(ms.ToArray()));
            }

            SnackBar.Add("Export Successful!", Severity.Success);
        }
    }
}
