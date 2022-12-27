using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Api;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsesController : ControllerBase
    {
        private readonly ResponseService _responseService;

        public ResponsesController(ResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet("ByUser")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            return Ok(await _responseService.GetByUser(userId));
        }

        [HttpGet("ByOffer")]
        public async Task<IActionResult> GetByOffer(long offerId)
        {
            return Ok(await _responseService.GetByOffer(offerId));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(ResponsePostDto response)
        {
            return Ok(
                await _responseService.Create(
                    response.OfferId,
                    response.UserId,
                    response.ResponseText!,
                    response.CurriculumVitae!
                ));
        }
    }
}