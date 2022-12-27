using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Api;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LocationsController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationsController([FromServices] LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("All")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin+","+CustomRoles.User+","+CustomRoles.Recruiter)]
        public async Task<IActionResult> GetLocations()
        {
            return Ok((await _locationService.GetLocations()).Select(l => new LocationDto
            {
                LocationId = l.LocationId,
                LocationName = l.LocationName
            }));
        }

        [HttpPost("Create")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> Create(string name)
        {
            return Ok(await _locationService.Create(name));
        }
    }
}