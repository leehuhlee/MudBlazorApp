namespace MudBlazorApp.Client.Routes
{
    public static class AuthEndPoints
    {
        public static string GetUser = "api/auth/user";
        public static string GetProfilePicture = "api/auth/profile-picture";

        public static string Register = "api/auth/register";
        public static string Login = "api/auth/login";
        public static string Logout = "api/auth/logout";

        public static string ChangePassword = "api/auth/change-password";
        public static string UpdateUserDetails = "api/auth/change-profile";
        public static string UpdateProfilePicture = "api/auth/change-profile-picture";
    }
}
