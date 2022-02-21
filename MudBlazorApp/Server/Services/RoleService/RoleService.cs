using MudBlazorApp.Server.Services.UserService;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly DataContext _context;
        private readonly IUserService _userService;

        public RoleService(DataContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<ServiceResponse<Role>> GetRoleAsync(int roleId)
        {
            var findRole = await _context.Roles.FindAsync(roleId);
            return new ServiceResponse<Role> { Data = findRole };
        }

        public async Task<ServiceResponse<List<Role>>> GetRolesAsync()
        {
            var allRole = await _context.Roles.ToListAsync();
            return new ServiceResponse<List<Role>> { Data = allRole };
        }

        public async Task<ServiceResponse<bool>> CreateRoleAsync(Role role)
        {
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Create Role Successful!", Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateRoleAsync(Role role)
        {
            var findRole = await _context.Roles.FindAsync(role.Id);
            findRole.Name = role.Name;
            findRole.Description = role.Description;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Update Role Successful!", Data = true };
        }

        public async Task<ServiceResponse<bool>> UpdateUserRoleAsync(UserRole userRole)
        {
            var findUser = await _userService.GetUserDetailsAsync(userRole.UserId);
            var findRole = await _context.Roles.FindAsync(userRole.RoleId);
            findUser.Data.RoleId = findRole.Id;
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Update User's Role Successful!", Data = true };
        }

        public async Task<ServiceResponse<bool>> DeleteRoleAsync(int roleId)
        {
            var findRole = await _context.Roles.FindAsync(roleId);
            _context.Roles.Remove(findRole);
            await _context.SaveChangesAsync();
            return new ServiceResponse<bool> { Message = "Delete Role Successful!", Data = true };
        }
    }
}
