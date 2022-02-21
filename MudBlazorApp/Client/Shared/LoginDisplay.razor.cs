using MudBlazor;
using MudBlazorApp.Client.Shared.Dialogs;

namespace MudBlazorApp.Client.Shared
{
    public partial class LoginDisplay
    {
        private async Task Logout()
        {
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            DialogService.Show<LogoutConfirmation>("Logout", options);
        }
    }
}
