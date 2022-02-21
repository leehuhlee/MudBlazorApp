using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;

namespace MudBlazorApp.Client.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _http;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(HttpClient http, AuthenticationStateProvider authStateProvider)
        {
            _http = http;
            _authStateProvider = authStateProvider;
        }
        public async Task<User> GetUser()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<User>>(AuthEndPoints.GetUser);
            if (response != null && response.Data != null)
                return response.Data;
            return new User();
        }

        public async Task<string> GetProfilePictureAsync()
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>(AuthEndPoints.GetProfilePicture);
            if (result != null && result.Data != null)
                return result.Data;
            return null;
        }

        public async Task<bool> IsUserAuthenticated()
        {
            return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
        }
        public async Task<ServiceResponse<int>> Register(UserRegister request)
        {
            var result = await _http.PostAsJsonAsync(AuthEndPoints.Register, request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }

        public async Task<ServiceResponse<string>> Login(UserLogin request)
        {
            var result = await _http.PostAsJsonAsync(AuthEndPoints.Login, request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<bool>> Logout(User user)
        {
            var result = await _http.PostAsJsonAsync(AuthEndPoints.Logout, user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> ChangePassword(UserChangePassword request)
        {
            var result = await _http.PostAsJsonAsync(AuthEndPoints.ChangePassword, request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<string>> UpdateUserDetails(User user)
        {
            var result = await _http.PutAsJsonAsync(AuthEndPoints.UpdateUserDetails, user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<bool>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
        {
            var result = await _http.PutAsJsonAsync(AuthEndPoints.UpdateProfilePicture, request);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
