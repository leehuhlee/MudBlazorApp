using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.RoleService
{
    public interface IRoleService
    {
        Task<Role> GetRoleAsync(int roleId);
        Task<List<Role>> GetRolesAsync();
        Task<int> GetRoleCountAsync();
        Task<double[]> GetRoleMonthlyCountAsync();
        Task<ServiceResponse<bool>> CreateRoleAsync(Role role);
        Task<ServiceResponse<bool>> UpdateRoleAsync(Role role);
        Task<ServiceResponse<bool>> UpdateUserRoleAsync(UserRole userRole);
        Task<ServiceResponse<bool>> DeleteRoleAsync(int roleId);
    }
}
