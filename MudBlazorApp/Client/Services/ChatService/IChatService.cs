using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Client.Services.ChatService
{
    public interface IChatService
    {
        Task<List<User>> GetUsersAsync();
        Task<List<ChatMessage>> GetConversationAsync(int contactId);
        Task SaveMessageAsync(ChatMessage message);
    }
}
