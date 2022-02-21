using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly HttpClient _http;

        public UserService(HttpClient http)
        {
            _http = http;
        }

        public async Task<User> GetUserDetailsAsync(int userId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<User>>(UserEndPoints.GetUserDetails(userId));
            if (response != null && response.Data != null)
                return response.Data;
            return new User();
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>(UserEndPoints.GetUsers);
            if (response != null && response.Data != null)
                return response.Data;
            return new List<User>();
        }

        public async Task<int> GetUserCountAsync()
        {
            var userList = await GetUsersAsync();
            return userList.Count;
        }

        public async Task<double[]> GetUserMonthlyCountAsync()
        {
            var userList = await GetUsersAsync();
            double[] UserMonthlyCount = new double[12];
            foreach (var user in userList)
                UserMonthlyCount[user.CreatedDate.Month - 1]++;
            return UserMonthlyCount;
        }

        public async Task<string> GetUserProfilePictureAsync(int userId)
        {
            var result = await _http.GetFromJsonAsync<ServiceResponse<string>>(UserEndPoints.GetUserProfilePicture(userId));
            if (result != null && result.Data != null)
                return result.Data;
            return null;
        }

        public async Task<ServiceResponse<bool>> UpdateUserAsync(User user)
        {
            var result = await _http.PutAsJsonAsync(UserEndPoints.UpdateUser, user);
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task<ServiceResponse<bool>> DeleteUserAsync(int userId)
        {
            var result = await _http.DeleteAsync(UserEndPoints.DeleteUser(userId));
            return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }
    }
}
