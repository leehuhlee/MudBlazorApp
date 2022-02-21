using Microsoft.AspNetCore.Components;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class UserProfilePage
    {
        [Parameter] public string UserUrl { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        private int UserId;
        private User User;

        private bool _loaded;
        private char _firstLetterOfName;

        protected override async Task OnInitializedAsync()
        {
            UserId = int.Parse(UserUrl);
            User = await UserService.GetUserDetailsAsync(UserId);

            if (User != null)
            {
                Title = $"{User.GetUserName()}'s Profile";
                Description = $"Manage {User.GetUserName()}'s Profile";

                if (User.FirstName.Length > 0)
                {
                    _firstLetterOfName = User.FirstName[0];
                }
            }

            _loaded = true;
        }
    }
}
