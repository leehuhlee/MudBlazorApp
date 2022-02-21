using Microsoft.AspNetCore.Components;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Shared.Components
{
    public partial class UserCard
    {
        [Parameter] public string Class { get; set; }
        private string UserName { get; set; }
        private char FirstLetterOfName { get; set; }
        private User user = new User();

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            user = await AuthService.GetUser();

            UserName = user.GetUserName();
            if (UserName.Length > 0)
            {
                FirstLetterOfName = UserName[0];
            }
        }
    }
}
