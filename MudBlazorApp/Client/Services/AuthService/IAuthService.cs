using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;

namespace MudBlazorApp.Client.Services.AuthService
{
    public interface IAuthService
    {
        Task<User> GetUser();
        Task<string> GetProfilePictureAsync();
        Task<bool> IsUserAuthenticated();
        Task<ServiceResponse<int>> Register(UserRegister request);
        Task<ServiceResponse<string>> Login(UserLogin request);
        Task<ServiceResponse<bool>> Logout(User user);
        Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request);
        Task<ServiceResponse<string>> UpdateUserDetails(User user);
        Task<ServiceResponse<bool>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request);
    }
}
