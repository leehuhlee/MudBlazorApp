namespace MudBlazorApp.Client.Routes
{
    public static class UserEndPoints
    {
        public static string GetUsers = "api/user/users";
        public static string UpdateUser = "api/user/update";

        public static string GetUserDetails(int userId)
        {
            return $"api/user/users/{userId}";
        }

        public static string GetUserProfilePicture(int userId)
        {
            return $"api/user/users/profile-picture/{userId}";
        }

        public static string DeleteUser(int userId)
        {
            return $"api/user/delete/{userId}";
        }
    }
}
