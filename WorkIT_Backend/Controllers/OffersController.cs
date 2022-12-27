﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using WorkIT_Backend.Api;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OffersController : ControllerBase
    {
        private readonly SecurityService _securityService;
        private readonly OfferService _offerService;

        public OffersController(SecurityService securityService, OfferService offerService)
        {
            _securityService = securityService;
            _offerService = offerService;
        }

        [HttpGet("ByCategory")]
        public async Task<IActionResult> GetByCategory(long categoryId)
        {
            return Ok(OfferToDto(await _offerService.GetByCategory(categoryId)));
        }

        [HttpGet("ByUser")]
        public async Task<IActionResult> GetByUser(long userId)
        {
            return Ok(OfferToDto(await _offerService.GetByUser(userId)));
        }

        [HttpGet("MinSalary")]
        public async Task<IActionResult> GetByMinSalary(double salaryMin)
        {
            return Ok(OfferToDto(await _offerService.GetByMinSalary(salaryMin)));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOffer(OfferPostDto offer)
        {
            return Ok(
                await _offerService.Create(
                    offer.OfferName!,
                    offer.OfferDescription!,
                    offer.UserId,
                    offer.CategoryId,
                    offer.LocationId,
                    offer.SalaryMin,
                    offer.SalaryMax
                ));
        }

        [NonAction]
        public List<OfferDto> OfferToDto(List<Offer> offers)
        {
            return offers.Select(o => new OfferDto
            {
                OfferId = o.OfferId,
                OfferName = o.OfferName,
                OfferDescription = o.OfferDescription,
                SalaryMin = o.SalaryMin,
                SalaryMax = o.SalaryMax,
                User = new UserDto
                {
                    UserId = o.User!.UserId,
                    Role = new RoleDto
                    {
                        RoleId = o.User.Role.RoleId,
                        Name = o.User.Role.Name
                    },
                    UserName = o.User.UserName
                },
                Category = new CategoryDto
                {
                    CategoryId = o.Category!.CategoryId,
                    CategoryName = o.Category!.CategoryName
                },
                Location = new LocationDto
                {
                    LocationId = o.Location!.LocationId,
                    LocationName = o.Location!.LocationName
                },
                Responses = o.Responses.Select(r => new ResponseDto
                {
                    ResponseId = r.ResponseId,
                    ResponseText = r.ResponseText,
                    CurriculumVitae = r.CurriculumVitae,
                    User = new UserDto
                    {
                        UserId = r.User!.UserId, UserName = r.User.UserName,
                        Role = new RoleDto {RoleId = r.User.Role.RoleId, Name = r.User.Role.Name}
                    }
                }).ToList()
            }).ToList();
        }
    }
}