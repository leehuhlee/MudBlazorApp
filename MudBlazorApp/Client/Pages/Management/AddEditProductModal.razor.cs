using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Management
{
    public partial class AddEditProductModal
    {
		[CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
		[Parameter] public Product Product { get; set; } = new Product();
		private FluentValidationValidator _fluentValidationValidator;
		private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });
		private List<Brand> brandList = new List<Brand>();
		private string brandName { get; set; }

        protected override async Task OnInitializedAsync()
        {
            brandList = await BrandService.GetBrandsAsync();
			var findBrand = await BrandService.GetBrandAsync(Product.BrandId);
			brandName = findBrand.Name;

			HubConnection = HubConnection.TryInitialize(NavigationManager);
			if (HubConnection.State == HubConnectionState.Disconnected)
			{
				await HubConnection.StartAsync();
			}
		}

        protected async Task Save()
		{
			if (Product.Id != 0)
			{
				var result = await ProductService.UpdateProductAsync(Product);
				SnackBar.Add(result.Message, Severity.Success);
			}
			else
			{
				var result = await ProductService.CreateProductAsync(Product);
				SnackBar.Add(result.Message, Severity.Success);
			}
			MudDialog.Close();
		}

		public void Cancel()
		{
			MudDialog.Cancel();
		}
	}
}
