namespace MudBlazorApp.Client.Routes
{
    public static class RoleEndPoints
    {
        public static string GetRoles = "api/role/roles";
        public static string CreateRole = "api/role/create";
        public static string UpdateRole = "api/role/update";
        public static string UpdateUserRole = "api/role/update/user-role";

        public static string GetRole(int roleId)
        {
            return $"api/role/roles/{roleId}";
        }

        public static string DeleteRole(int roleId)
        {
            return $"api/role/roles/delete/{roleId}";
        }

    }
}
