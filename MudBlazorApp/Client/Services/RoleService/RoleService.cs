using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly HttpClient _http;

        public RoleService(HttpClient http)
        {
            _http = http;
        }

        public async Task<Role> GetRoleAsync(int roleId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<Role>>(RoleEndPoints.GetRole(roleId));
            if (response != null && response.Data != null)
                return response.Data;
            return new Role();
        }

        public async Task<List<Role>> GetRolesAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<Role>>>(RoleEndPoints.GetRoles);
            if (response != null && response.Data != null)
                return response.Data;
            return new List<Role>();
        }

        public async Task<int> GetRoleCountAsync()
        {
            var roleList = await GetRolesAsync();
            return roleList.Count;
        }

        public async Task<double[]> GetRoleMonthlyCountAsync()
        {
            var roleList = await GetRolesAsync();
            double[] RoleMonthlyCount = new double[12];
            foreach (var role in roleList)
                RoleMonthlyCount[role.CreatedDate.Month - 1]++;
            return RoleMonthlyCount;
        }

        public async Task<ServiceResponse<bool>> CreateRoleAsync(Role role)
        {
            var result = await _http.PostAsJsonAsync(RoleEndPoints.CreateRole, role);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> UpdateRoleAsync(Role role)
        {
            var result = await _http.PutAsJsonAsync(RoleEndPoints.UpdateRole, role);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> UpdateUserRoleAsync(UserRole userRole)
        {
            var result = await _http.PutAsJsonAsync(RoleEndPoints.UpdateUserRole, userRole);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> DeleteRoleAsync(int roleId)
        {
            var result = await _http.DeleteAsync(RoleEndPoints.DeleteRole(roleId));
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

    }
}
