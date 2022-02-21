using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.UserService
{
    public interface IUserService
    {
        Task<User> GetUserDetailsAsync(int userId);
        Task<List<User>> GetUsersAsync();
        Task<int> GetUserCountAsync();
        Task<double[]> GetUserMonthlyCountAsync();
        Task<string> GetUserProfilePictureAsync(int userId);
        Task<ServiceResponse<bool>> UpdateUserAsync(User user);
        Task<ServiceResponse<bool>> DeleteUserAsync(int userId);
    }
}
