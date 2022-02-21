using Blazored.FluentValidation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class AddEditRoleModal
    {
        [CascadingParameter] private MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }
        [Parameter] public Role Role { get; set; } = new Role();
        private FluentValidationValidator _fluentValidationValidator;
		private bool Validated => _fluentValidationValidator.Validate(options => { options.IncludeAllRuleSets(); });

		protected async Task Save()
		{
			if (Role.Id != 0)
			{
				var result = await RoleService.UpdateRoleAsync(Role);
				SnackBar.Add(result.Message, Severity.Success);
			}
			else
			{
				var result = await RoleService.CreateRoleAsync(Role);
				SnackBar.Add(result.Message, Severity.Success);
			}
			MudDialog.Close();
		}

		public void Cancel()
		{
			MudDialog.Cancel();
		}

		protected override async Task OnInitializedAsync()
		{
			HubConnection = HubConnection.TryInitialize(NavigationManager);
			if (HubConnection.State == HubConnectionState.Disconnected)
			{
				await HubConnection.StartAsync();
			}
		}
	}
}
