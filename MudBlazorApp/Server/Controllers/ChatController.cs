using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Server.Services.ChatService;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsersAsync()
        {
            var response = await _chatService.GetUsersAsync();

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("{contactId}")]
        public async Task<ActionResult<ServiceResponse<List<ChatMessage>>>> GetConversationAsync(int contactId)
        {
            var response = await _chatService.GetConversationAsync(contactId);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("save")]
        public async Task<ActionResult<ServiceResponse<bool>>> SaveMessageAsync(ChatMessage message)
        {
            var response = await _chatService.SaveMessageAsync(message);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
