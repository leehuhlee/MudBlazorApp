using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class UserRolePage
    {
        [Parameter] public string UserUrl { get; set; }
        [Parameter] public string Title { get; set; }
        [Parameter] public string Description { get; set; }

        private int UserId;
        private User User;
        protected List<Role> roleList = new List<Role>();

        private int UserRoleId;
        protected List<UserRole> userRoleList = new List<UserRole>();
        protected List<UserRole> searchUserRoleData = new List<UserRole>();


        private string SearchString = "";

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            UserId = int.Parse(UserUrl);
            await Init();
            Title = User.GetUserName();
            Description = $"Manage {Title}'s Role";

            _loaded = true;
        }

        private async Task Init()
        {
            userRoleList.Clear();
            User = await UserService.GetUserDetailsAsync(UserId);
            roleList = await RoleService.GetRolesAsync();
            UserRoleId = User.RoleId;
            GetUserRoles();
        }

        protected void GetUserRoles()
        {
            roleList.ForEach(role =>
                userRoleList.Add(new UserRole()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    Selected = role.Id == UserRoleId ? true : false,
                    UserId = role.Id == UserRoleId ? User.Id : 0
                }));
            searchUserRoleData = userRoleList;
        }

        protected void FilterUserRole(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                OnSearch();
            }
            else
            {
                userRoleList = searchUserRoleData;
            }
        }

        protected void OnSearch()
        {
            userRoleList = searchUserRoleData
                .Where(x => x.RoleName.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .ToList();
        }

        public void ResetSearch()
        {
            SearchString = string.Empty;
            userRoleList = searchUserRoleData;
        }

        public async Task ChangeUserRole(UserRole userRole)
		{
            userRole.UserId = User.Id;
            var result = await RoleService.UpdateUserRoleAsync(userRole);
            SnackBar.Add(result.Message, Severity.Success);
            await Init();
        }
    }
}
