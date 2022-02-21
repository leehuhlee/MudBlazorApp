using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.ChatService
{
    public interface IChatService
    {
        Task<ServiceResponse<List<User>>> GetUsersAsync();
        Task<ServiceResponse<List<ChatMessage>>> GetConversationAsync(int contactId);
        Task<ServiceResponse<ChatMessage>> SaveMessageAsync(ChatMessage message);
        Task UpdateUserProfilePictureUrlAsync(int userId, string newProfilePictureUrl);
    }
}
