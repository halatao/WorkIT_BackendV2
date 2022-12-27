using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly LocationService _locationService;

        public LocationsController(LocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetLocations()
        {
            return Ok(await _locationService.GetLocations());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(string name)
        {
            return Ok(await _locationService.Create(name));
        }
    }
}