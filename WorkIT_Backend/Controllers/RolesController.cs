using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly RoleService _roleService;
        private readonly SecurityService _securityService;

        public RolesController(RoleService roleService, SecurityService securityService)
        {
            _roleService = roleService;
            _securityService = securityService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetRoles()
        {
            return Ok(await _roleService.GetRoles());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRole(string role)
        {
            return Ok(await _roleService.Create(role));
        }
    }
}