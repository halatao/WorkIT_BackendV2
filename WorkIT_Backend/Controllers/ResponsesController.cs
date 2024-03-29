﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Api;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ResponsesController : ControllerBase
    {
        private readonly ResponseService _responseService;

        public ResponsesController([FromServices] ResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet("ByUser")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.User + "," + CustomRoles.Recruiter)]
        public async Task<IActionResult> GetByUser(long userId)
        {
            return Ok(ResponseToDto(await _responseService.GetByUser(userId)));
        }

        [HttpGet("ByOffer")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.Recruiter)]
        public async Task<IActionResult> GetByOffer(long offerId)
        {
            return Ok(ResponseToDto(await _responseService.GetByOffer(offerId)));
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.User)]
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

        [NonAction]
        public List<ResponseDto> ResponseToDto(List<Response> responses)
        {
            return responses.Select(r => new ResponseDto
            {
                ResponseId = r.ResponseId,
                CurriculumVitae = r.CurriculumVitae,
                ResponseText = r.ResponseText,
                User = new UserSimpleDto
                {
                    UserId = r.UserId,
                    UserName = r.User?.UserName,
                    Role = new RoleDto
                    {
                        RoleId = r.User!.RoleId,
                        Name = r.User.Role.Name
                    }
                }
            }).ToList();
        }
    }
}