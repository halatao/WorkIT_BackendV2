using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RolesController([FromServices] RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("All")]
        [AllowAnonymous] //[Authorize(Roles = "RECRUITER,USER,ADMIN")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetRoles());
        }

        [HttpPost("Create")]
        [AllowAnonymous] //[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateRole(string role)
        {
            return Ok(await _roleService.Create(role));
        }
    }
}