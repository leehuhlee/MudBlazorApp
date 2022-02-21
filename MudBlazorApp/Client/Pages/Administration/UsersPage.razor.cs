using MudBlazor;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class UsersPage
    {
        protected List<User> userList = new List<User>();

        protected List<UserTable> userTableList = new List<UserTable>();
        protected List<UserTable> searchUserTableData = new List<UserTable>();
        protected User user = new User();
        protected string SearchString { get; set; } = string.Empty;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await Init();
            _loaded = true;
        }

        protected async Task Init()
        {
            userTableList.Clear();
            userList = await UserService.GetUsersAsync();
            foreach(var user in userList)
            {
                var roleName = (await RoleService.GetRoleAsync(user.RoleId)).Name;
                userTableList.Add(new UserTable()
                {
                    Id = user.Id,
                    UserName = user.GetUserName(),
                    Email = user.Email,
                    IsOnline = user.IsOnline,
                    Role = roleName
                });
            }
            searchUserTableData = userTableList;
        }

        protected void FilterUser(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                OnSearch();
            }
            else
            {
                userTableList = searchUserTableData;
            }
        }

        protected void OnSearch()
        {
            userTableList = searchUserTableData
                .Where(x => x.UserName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .ToList();
        }

        public void ResetSearch()
        {
            SearchString = string.Empty;
            userTableList = searchUserTableData;
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<AddUserModal>("Create", parameters, options);
            await dialog.Result;

            await Init();
        }

        private void GoToProfile(int userId)
        {
            NavigationManager.NavigateTo($"profile/{userId}");
        }

        private void GoToUserRole(int userId)
        {
            NavigationManager.NavigateTo($"role/{userId}");
        }

        private async Task Delete(int roleId)
        {
            string deleteContent = "Delete User";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, roleId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var response = await UserService.DeleteUserAsync(roleId);
                SnackBar.Add(response.Message, Severity.Success);
                await Init();
            }
        }
    }
}
