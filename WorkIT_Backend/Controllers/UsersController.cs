using System.Diagnostics.Tracing;
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
    [Authorize]
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
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _userService.GetUsers());
        }

        [HttpGet("ById")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.User + "," + CustomRoles.Recruiter)]
        public async Task<IActionResult> GetById(long userId)
        {
            var user = await _userService.GetById(userId);

            return Ok(UserToDto(user));
        }

        [HttpGet("ByUsername")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.User + "," + CustomRoles.Recruiter)]
        public async Task<IActionResult> GetByUsername(string username)
        {
            var user = await _userService.GetByUsername(username);

            return Ok(UserToDto(user));
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

            return Ok(_securityService.BuildJwtToken(user));
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> CreateUser(UserLoginDto user)
        {
            return Ok(_securityService.BuildJwtToken(await _userService.Create(user.UserName!, user.Password!,
                user.Role!)));
        }

        [NonAction]
        public UserFullDto UserToDto(User user)
        {
            return new UserFullDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Role = new RoleDto
                {
                    RoleId = user.Role.RoleId,
                    Name = user.Role.Name
                },
                Offers = user.Offers.Select(o => new OfferSimpleDto
                {
                    OfferId = o.OfferId,
                    OfferName = o.OfferName
                }).ToList(),
                Responses = user.Responses.Select(r => new ResponseSimpleDto
                {
                    ResponseId = r.ResponseId,
                    Offer = new OfferSimpleDto {OfferId = r.Offer!.OfferId, OfferName = r.Offer.OfferName}
                }).ToList()
            };
        }
    }
}