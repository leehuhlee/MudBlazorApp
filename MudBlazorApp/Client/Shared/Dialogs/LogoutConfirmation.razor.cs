using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Shared.Dialogs
{
    public partial class LogoutConfirmation
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        async Task Submit()
        {
            var user = await AuthService.GetUser();
            await HubConnection.SendAsync(ApplicationConstants.SignalR.OnDisconnect, user.Id);
            await AuthService.Logout(user);
            await LocalStorage.RemoveItemAsync("authToken");
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("/login");
            MudDialog.Close(DialogResult.Ok(true));
        }

        void Cancel() => MudDialog.Cancel();

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

