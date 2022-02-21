using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;

namespace MudBlazorApp.Server.Services.AuthService
{
    public interface IAuthService
    {
        int GetUserId();
        string GetUserEmail();
        Task<bool> UserExists(string email);

        Task<ServiceResponse<User>> GetUserAsync();
        Task<ServiceResponse<string>> GetProfilePictureAsync(int userId);

        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string email, string password);
        Task<ServiceResponse<bool>> Logout(int userId);

        Task<ServiceResponse<bool>> ChangePassword(int userId, string password, string newPassword);
        Task<ServiceResponse<string>> UpdateUserDetailsAsync(User user);
        Task<ServiceResponse<bool>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request, int userId);
    }
}
