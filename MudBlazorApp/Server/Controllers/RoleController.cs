using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MudBlazorApp.Server.Services.RoleService;
using MudBlazorApp.Shared.Models;

namespace MudBlazorApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("roles/{roleId}")]
        public async Task<ActionResult<ServiceResponse<Role>>> GetRoleAsync(int roleId)
        {
            var response = await _roleService.GetRoleAsync(roleId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<ServiceResponse<List<Role>>>> GetRolesAsync()
        {
            var response = await _roleService.GetRolesAsync();
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceResponse<bool>>> CreateRoleAsync(Role role)
        {
            var response = await _roleService.CreateRoleAsync(role);
            if(!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateRoleAsync(Role role)
        {
            var response = await _roleService.UpdateRoleAsync(role);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpPut("update/user-role")]
        public async Task<ActionResult<ServiceResponse<bool>>> UpdateUserRoleAsync(UserRole userRole)
        {
            var response = await _roleService.UpdateUserRoleAsync(userRole);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }

        [HttpDelete("delete/{roleId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteRoleAsync(int roleId)
        {
            var response = await _roleService.DeleteRoleAsync(roleId);
            if (!response.Success)
                return BadRequest(response);
            return Ok(response);
        }
    }
}
