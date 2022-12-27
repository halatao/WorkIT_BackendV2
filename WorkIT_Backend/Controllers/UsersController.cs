using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Api;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly SecurityService _securityService;

        public UsersController([FromServices] SecurityService securityService, [FromServices] UserService userService)
        {
            _securityService = securityService;
            _userService = userService;
        }

        [HttpGet("All")]
        //[Authorize(Roles = "ADMIN")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            User user;
            try
            {
                user = await _userService.GetUserByCredentials(username, password);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var token = _securityService.BuildJwtToken(user);
            return Ok(token);
        }

        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(UserLoginDto user)
        {
            return Ok(await _userService.Create(user.UserName!, user.Password!, user.Role!));
        }
    }
}