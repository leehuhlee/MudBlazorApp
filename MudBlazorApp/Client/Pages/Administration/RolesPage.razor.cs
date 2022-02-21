using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using MudBlazorApp.Client.Extensions;
using MudBlazorApp.Shared.Constants;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Pages.Administration
{
    public partial class RolesPage
    {
        [CascadingParameter] private HubConnection HubConnection { get; set; }

        protected List<Role> roleList = new List<Role>();
        protected List<Role> searchRoleData = new List<Role>();
        protected Role role = new Role();

        protected string SearchString { get; set; } = string.Empty;

        private bool _loaded;

        protected override async Task OnInitializedAsync()
        {
            await GetRoles();
            _loaded = true;

            HubConnection = HubConnection.TryInitialize(NavigationManager);
            if (HubConnection.State == HubConnectionState.Disconnected)
            {
                await HubConnection.StartAsync();
            }
        }

        protected async Task GetRoles()
        {
            roleList = await RoleService.GetRolesAsync();
            searchRoleData = roleList;
        }

        protected void FilterRole(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                SearchString = searchString;
                OnSearch();
            }
            else
            {
                roleList = searchRoleData;
            }
        }

        protected void OnSearch()
        {
            roleList = searchRoleData
                .Where(x => x.Name.IndexOf(SearchString, StringComparison.OrdinalIgnoreCase) != -1)
                .ToList();
        }

        public void ResetSearch()
        {
            SearchString = string.Empty;
            roleList = searchRoleData;
        }

        private async Task InvokeModal(int id = 0)
        {
            var parameters = new DialogParameters();
            if (id != 0)
            {
                role = roleList.FirstOrDefault(c => c.Id == id);
                if (role != null)
                {
                    parameters.Add(nameof(AddEditRoleModal.Role), new Role
                    {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description
                    });
                }
            }

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<AddEditRoleModal>(id == 0 ? "Create" : "Edit", parameters, options);
            await dialog.Result;

            await GetRoles();
        }

        private async Task Delete(int roleId)
        {
            string deleteContent = "Delete Role";
            var parameters = new DialogParameters
            {
                {nameof(Shared.Dialogs.DeleteConfirmation.ContentText), string.Format(deleteContent, roleId)}
            };
            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true, DisableBackdropClick = true };
            var dialog = DialogService.Show<Shared.Dialogs.DeleteConfirmation>("Delete", parameters, options);
            var result = await dialog.Result;

            if (!result.Cancelled)
            {
                var response = await RoleService.DeleteRoleAsync(roleId);
                if (response.Success)
                {
                    await HubConnection.SendAsync(ApplicationConstants.SignalR.SendUpdateDashboard);
                    SnackBar.Add(response.Message, Severity.Success);
                    await GetRoles();
                }
                else
                {
                    await GetRoles();
                    SnackBar.Add(response.Message, Severity.Error);
                }
            }
        }
    }
}
