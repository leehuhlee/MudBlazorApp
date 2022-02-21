using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using Microsoft.JSInterop;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Client.Extensions;

namespace MudBlazorApp.Client.Shared
{
    public partial class MainLayout
    {
		bool _drawerOpen = false;
		void DrawerToggle()
		{
			_drawerOpen = !_drawerOpen;
		}

		private int CurrentUserId { get; set; }
		private string FirstName { get; set; }
		private string SecondName { get; set; }
		private string Email { get; set; }
		private char FirstLetterOfName { get; set; }

		private HubConnection HubConnection;
		public bool IsConnected => HubConnection.State == HubConnectionState.Connected;
		protected override async Task OnInitializedAsync()
		{
			if (!(await AuthService.IsUserAuthenticated()))
				NavigationManager.NavigateTo("login");

			HubConnection = HubConnection.TryInitialize(NavigationManager);
			await HubConnection.StartAsync();
			HubConnection.On<string, int, int>(ApplicationConstants.SignalR.ReceiveChatNotification, (message, receiverUserId, senderUserId) =>
			{
				if (CurrentUserId == receiverUserId)
				{
					JSRuntime.InvokeAsync<string>("PlayAudio", "notification");
					SnackBar.Add(message, Severity.Info, config =>
					{
						config.VisibleStateDuration = 10000;
						config.HideTransitionDuration = 500;
						config.ShowTransitionDuration = 500;
						config.Action = "Chat?";
						config.ActionColor = Color.Info;
						config.Onclick = snackbar =>
						{
							NavigationManager.NavigateTo($"chat/{senderUserId}");
							return Task.CompletedTask;
						};
					});
				}
			});

			await LoadDataAsync();
		}

		private async Task LoadDataAsync()
		{
			var isUserAuthenticated = await AuthService.IsUserAuthenticated();
			if (!isUserAuthenticated)
				return;
			var currentUser = await AuthService.GetUser();
			if (currentUser == null) 
			{
				SnackBar.Add("You are logged out because the user with your Token has been deleted.", Severity.Error);
			}
			if (isUserAuthenticated)
			{
				CurrentUserId = currentUser.Id;
				FirstName = currentUser.FirstName;
				if (FirstName.Length > 0)
				{
					FirstLetterOfName = FirstName[0];
				}
				SecondName = currentUser.LastName;
				Email = currentUser.Email;

				await HubConnection.SendAsync(ApplicationConstants.SignalR.OnConnect, CurrentUserId);
			}
		}
	}
}
