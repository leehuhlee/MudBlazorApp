using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Models;
using System.Globalization;

namespace MudBlazorApp.Client.Pages.Management
{
    public partial class BrandsPage
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        private List<Brand> brandList = new List<Brand>();
        private List<Brand> serachBrandData = new List<Brand>();
        private Brand brand;

        private string SearchString;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await GetBrands();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(NavigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        private async Task GetBrands()
        {
            brandList = await BrandService.GetBrandsAsync();
            serachBrandData = brandList;
        }
        protected void FilterBrand(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                OnSearch();
            }
            else
            {
                brandList = serachBrandData;
            }
        }

        protected void OnSearch()
        {
            brandList = serachBrandData
                .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .ToList();
        }

        public void ResetSearch()
        {
            SearchString = string.Empty;
            brandList = serachBrandData;
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                brand = brandList.FirstOrDefault(c => c.Id == id);
                if (brand != null)
                {
                    parameters.Add(nameof(AddEditBrandModal.Brand), new Brand
                    {
                        Id = brand.Id,
                        Name = brand.Name,
                        Description = brand.Description
                    });
                }
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<AddEditBrandModal>(id == 0 ? "Create" : "Edit", parameters, options);
            await dialog.Result;

            await GetBrands();
        }

        private async Task Delete(int brandId)
        {
            string deleteContent = "Delete Brand";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, brandId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var response = await BrandService.DeleteBrandAsync(brandId);
                SnackBar.Add(response.Message, Severity.Success);
                await GetBrands();
            }
        }

        private async Task ExportToExcel()
        {
            var fileBytes = await BrandService.ExportBrandAsync(brandList);

            using (MemoryStream ms = new MemoryStream())
            {
                fileBytes.CopyTo(ms);
                var fileName = $"BrandList{DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)}.xlsx";
                await JSRuntime.InvokeAsync<object>("saveAsFile", fileName, Convert.ToBase64String(ms.ToArray()));
            }

            SnackBar.Add("Export Successful!", Severity.Success);
        }
    }
}
