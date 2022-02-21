using MudBlazorApp.Server.Services.UserService;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Services.ChatService
{
    public class ChatService : IChatService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public ChatService(DataContext context, IAuthService authService, IUserService userService)
        {
            _context = context;
            _authService = authService;
            _userService = userService;
        }

        public async Task<ServiceResponse<List<User>>> GetUsersAsync()
        {
            int userId = _authService.GetUserId();
            var allUsers = await _context.Users.Where(user => user.Id != userId).ToListAsync();
            return new ServiceResponse<List<User>>
            {
                Data = allUsers
            };
        }

        public async Task<ServiceResponse<List<ChatMessage>>> GetConversationAsync(int contactId)
        {
            var user = (await _authService.GetUserAsync()).Data;
            var contactUser = (await _userService.GetUserDetailsAsync(contactId)).Data;
            var messages = await _context.ChatMessages
                    .Where(h => (h.FromUserId == contactId && h.ToUserId == user.Id) || (h.FromUserId == user.Id && h.ToUserId == contactId))
                    .OrderBy(a => a.CreatedDate)
                    .Include(a => a.FromUser)
                    .Include(a => a.ToUser)
                    .Select(x => new ChatMessage
                    {
                        FromUserId = x.FromUserId,
                        Message = x.Message,
                        CreatedDate = x.CreatedDate,
                        Id = x.Id,
                        ToUserId = x.ToUserId,
                        ToUser = x.ToUser,
                        FromUser = x.FromUser,
                        FromUserProfilePictureUrl = x.FromUserProfilePictureUrl
                    }).ToListAsync();

            foreach(var message in messages)
            {
                if(message.FromUserId == user.Id && message.FromUserProfilePictureUrl != user.ProfilePictureDataUrl)
                    message.FromUserProfilePictureUrl = user.ProfilePictureDataUrl;
                else if(message.FromUserId == contactId && message.FromUserProfilePictureUrl != contactUser.ProfilePictureDataUrl)
                    message.FromUserProfilePictureUrl = contactUser.ProfilePictureDataUrl;
            }
            await _context.SaveChangesAsync();

            return new ServiceResponse<List<ChatMessage>>
            {
                Data = messages
            };
        }

        public async Task<ServiceResponse<ChatMessage>> SaveMessageAsync(ChatMessage message)
        {
            var userId = _authService.GetUserId();
            message.FromUserId = userId;
            message.CreatedDate = DateTime.Now;
            message.ToUser = await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync();
             _context.ChatMessages.Add(message);
            await _context.SaveChangesAsync();
            return new ServiceResponse<ChatMessage>
            {
                Data = message
            };
        }

        public async Task UpdateUserProfilePictureUrlAsync(int userId, string newProfilePictureUrl)
        {
            var findMessageList = await _context.ChatMessages.Where(a => a.FromUserId == userId).ToListAsync();
            findMessageList.ForEach(a => a.FromUserProfilePictureUrl = newProfilePictureUrl);
            await _context.SaveChangesAsync();
        }
    }
}
