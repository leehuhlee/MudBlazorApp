using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.UserService
{
    public interface IUserService
    {
        Task<ServiceResponse<List<User>>> GetUsersAsync();
        Task<ServiceResponse<User>> GetUserDetailsAsync(int userId);
        Task<ServiceResponse<string>> GetUserProfilePictureAsync(int userId);
        Task<ServiceResponse<bool>> UpdateUserAsync(User user);
        Task<ServiceResponse<bool>> DeleteUserAsync(int userId);
    }
}
