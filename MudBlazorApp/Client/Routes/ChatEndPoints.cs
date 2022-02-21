namespace MudBlazorApp.Client.Routes
{
    public static class ChatEndPoints
    {
        public static string GetUsers = "api/chat/users";
        public static string SaveMessage = "api/chat/save";

        public static string GetConversation(int contactId)
        {
            return $"api/chat/{contactId}";
        }
    }
}
