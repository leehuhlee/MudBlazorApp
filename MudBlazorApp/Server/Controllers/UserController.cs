using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Server.Services.UserService;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<ServiceResponse<List<User>>>> GetUsersAsync()
        {
            var response = await _userService.GetUsersAsync();
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("users/{userId}")]
        public async Task<ActionResult<ServiceResponse<User>>> GetUserDetailsAsync(int userId)
        {
            var response = await _userService.GetUserDetailsAsync(userId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("users/profile-picture/{userId}")]
        public async Task<ActionResult<ServiceResponse<string>>> GetUserProfilePictureAsync(int userId)
        {
            var response = await _userService.GetUserProfilePictureAsync(userId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserAsync(User user)
        {
            var response = await _userService.UpdateUserAsync(user);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("delete/{userId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteUserAsync(int userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
