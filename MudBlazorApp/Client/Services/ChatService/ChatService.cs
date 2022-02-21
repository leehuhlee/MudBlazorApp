using MudBlazorApp.Client.Routes;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.ChatService
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _http;

        public ChatService(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<User>>>(ChatEndPoints.GetUsers);
            if (response != null && response.Data != null)
                return response.Data;
            return new List<User>();
        }

        public async Task<List<ChatMessage>> GetConversationAsync(int contactId)
        {
            var response = await _http.GetFromJsonAsync<ServiceResponse<List<ChatMessage>>>(ChatEndPoints.GetConversation(contactId));
            if (response != null && response.Data != null)
                return response.Data;
            return new List<ChatMessage>();
        }

        public async Task SaveMessageAsync(ChatMessage message)
        {
            await _http.PostAsJsonAsync(ChatEndPoints.SaveMessage, message);
        }

        
    }
}
