using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Shared.Models;
using MudBlazorApp.Shared.Requests;
using System.Security.Claims;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("user")]
        public async Task<ActionResult<ServiceResponse<User>>> GetUser()
        {
            var response = await _authService.GetUserAsync();
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("profile-picture")]
        [ResponseCache(NoStore = false, Location = ResponseCacheLocation.Client, Duration = 60)]
        public async Task<ActionResult<ServiceResponse<string>>> GetProfilePictureAsync()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _authService.GetProfilePictureAsync(userId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegister request)
        {
            var response = await _authService.Register(
                new User
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email
                },
                request.Password);

            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLogin request)
        {
            var response = await _authService.Login(request.Email, request.Password);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("logout"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> Logout(User user)
        {
            var response = await _authService.Logout(user.Id);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("change-password"), Authorize]
        public async Task<ActionResult<ServiceResponse<bool>>> ChangePassword(UserChangePassword request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _authService.ChangePassword(userId, request.Password, request.NewPassword);

            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("change-profile")]
        public async Task<ActionResult<ServiceResponse<string>>> UpdateUserDetailsAsync(User user)
        {
            var response = await _authService.UpdateUserDetailsAsync(user);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("change-profile-picture")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateProfilePictureAsync(UpdateProfilePictureRequest request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var response = await _authService.UpdateProfilePictureAsync(request, userId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
