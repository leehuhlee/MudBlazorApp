using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Management
{
    public partial class AddEditBrandModal
    {
		[CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
		[Parameter] public Brand Brand { get; set; } = new Brand();


		private FluentValidationValidator _fluentValidationValidator;
		private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

		protected async Task Save()
		{
			if (Brand.Id != 0)
			{
				var result = await BrandService.UpdateBrandAsync(Brand);
				SnackBar.Add(result.Message, Severity.Success);
			}
			else
			{
				var result = await BrandService.CreateBrandAsync(Brand);
				SnackBar.Add(result.Message, Severity.Success);
			}
			MudDialog.Close();
			await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
		}

		public void Cancel()
		{
			MudDialog.Cancel();
		}

		protected override async Task OnInitializedAsync()
		{
			await LoadDataAsync();
			HubConnection = HubConnection.TryInitialize(NavigationManager);
			if (HubConnection.State == HubConnectionState.Disconnected)
			{
				await HubConnection.StartAsync();
			}
		}

		private async Task LoadDataAsync()
		{
			await Task.CompletedTask;
		}
	}
}
