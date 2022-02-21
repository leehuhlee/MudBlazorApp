using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Communication
{
    public partial class ChatPage
    {
        [CascadingParameter] public HubConnection HubConnection { get; set; }
        [Parameter] public string CurrentMessage { get; set; }
        [Parameter] public int CurrentUserId { get; set; }
        [Parameter] public string CurrentUserEmail { get; set; }
        private List<ChatMessage> messages = new List<ChatMessage>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await JSRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }

        private async Task SubmitAsync()
        {
            if (!string.IsNullOrEmpty(ContactUrl))
            {
                ContactId = int.Parse(ContactUrl);
            }
            if (!string.IsNullOrEmpty(CurrentMessage) && ContactId != 0)
            {
                var chatHistory = new ChatMessage()
                {
                    Message = CurrentMessage,
                    ToUserId = ContactId,
                    CreatedDate = DateTime.Now,
                    FromUserId = CurrentUserId,
                    FromUserProfilePictureUrl = await UserService.GetUserProfilePictureAsync(CurrentUserId)
                };
                await ChatService.SaveMessageAsync(chatHistory);
                await HubConnection.SendAsync("SendMessageAsync", chatHistory, CurrentUserEmail);
                CurrentMessage = string.Empty;
            }

            await LoadUserChat(ContactId);
        }

        protected override async Task OnInitializedAsync()
        {
            HubConnection = HubConnection.TryInitialize(NavigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }

            HubConnection.On<string>(ApplicationConstants.SignalR.ConnectUser, (userId) =>
            {
                var connectedUser = UserList.Find(x => x.Id.Equals(userId));
                if (connectedUser is { IsOnline: false })
                {
                    connectedUser.IsOnline = true;
                    SnackBar.Add($"{connectedUser.GetUserName} Logged In.", Severity.Info);
                    StateHasChanged();
                }
            });
            HubConnection.On<string>(ApplicationConstants.SignalR.DisconnectUser, (userId) =>
            {
                var disconnectedUser = UserList.Find(x => x.Id.Equals(userId));
                if (disconnectedUser is { IsOnline: true })
                {
                    disconnectedUser.IsOnline = false;
                    SnackBar.Add($"{disconnectedUser.GetUserName} Logged Out.", Severity.Info);
                    StateHasChanged();
                }
            });
            HubConnection.On<ChatMessage, string, string>(ApplicationConstants.SignalR.ReceiveMessage, async (message, firstName, lastName) =>
            {
                if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId) || (ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                {
                    if ((ContactId == message.ToUserId && CurrentUserId == message.FromUserId))
                    {
                        messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new User() { FirstName = firstName, LastName = lastName, Email = CurrentUserEmail } });
                        await HubConnection.SendAsync(ApplicationConstants.SignalR.SendChatNotification, $"New Message From {firstName} {lastName}", ContactId, CurrentUserId);
                    }
                    else if ((ContactId == message.FromUserId && CurrentUserId == message.ToUserId))
                    {
                        messages.Add(new ChatMessage { Message = message.Message, CreatedDate = message.CreatedDate, FromUser = new User() { FirstName = firstName, LastName = lastName, Email = ContactEmail } });
                    }
                    await JSRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
                    StateHasChanged();
                }
            });
            await GetUsersAsync();
            var currentUser = await AuthService.GetUser();
            CurrentUserId = currentUser.Id;
            CurrentUserEmail = currentUser.Email;
            if (ContactId != 0)
            {
                await LoadUserChat(ContactId);
            }
        }
        public List<User> UserList = new List<User>();
        [Parameter] public string ContactEmail { get; set; }
        [Parameter] public int ContactId { get; set; }
        [Parameter] public string ContactUrl { get; set; }

        async Task LoadUserChat(int userId)
        {
            var contact = await UserService.GetUserDetailsAsync(userId);
            ContactId = contact.Id;
            ContactEmail = contact.Email;
            ContactUrl = ContactId.ToString();
            NavigationManager.NavigateTo($"chat/{ContactUrl}");
            messages = new List<ChatMessage>();
            messages = await ChatService.GetConversationAsync(ContactId);
        }

        private async Task GetUsersAsync()
        {
            UserList = await ChatService.GetUsersAsync();
        }

        private async Task OnKeyPressInChat(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
            {
                await SubmitAsync();
            }
        }

        private Color GetUserStatusBadgeColor(bool isOnline)
        {
            switch (isOnline)
            {
                case false:
                    return Color.Error;
                case true:
                    return Color.Success;
            }
        }
    }
}
