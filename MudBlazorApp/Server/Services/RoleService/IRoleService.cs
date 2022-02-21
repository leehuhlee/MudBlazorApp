using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.RoleService
{
    public interface IRoleService
    {
        Task<ServiceResponse<List<Role>>> GetRolesAsync();
        Task<ServiceResponse<Role>> GetRoleAsync(int roldId);
        Task<ServiceResponse<bool>> CreateRoleAsync(Role role);
        Task<ServiceResponse<bool>> UpdateRoleAsync(Role role);
        Task<ServiceResponse<bool>> UpdateUserRoleAsync(UserRole userRole);
        Task<ServiceResponse<bool>> DeleteRoleAsync(int roleId);
    }
}
