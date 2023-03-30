using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Api;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly SecurityService _securityService;
    private readonly UserService _userService;

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
    public async Task<IActionResult> Login(UserLoginDto login)
    {
        User user;
        try
        {
            user = await _userService.GetUserByCredentials(login.Username, login.Password);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok(_securityService.BuildJwtToken(user));
    }

    [HttpPost("Create")]
    [AllowAnonymous]
    public async Task<IActionResult> CreateUser(UserRegisterDto user)
    {
        if (user.Role == CustomRoles.Admin)
            return BadRequest();
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
                Offer = new OfferSimpleDto { OfferId = r.Offer!.OfferId, OfferName = r.Offer.OfferName }
            }).ToList()
        };
    }
}