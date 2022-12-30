using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Build.Framework;
using WorkIT_Backend.Api;
using WorkIT_Backend.Model;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OffersController : ControllerBase
    {
        private readonly OfferService _offerService;

        public OffersController([FromServices] OfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet("All")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin+","+CustomRoles.User+","+CustomRoles.Recruiter)]
        public async Task<IActionResult> GetOffers()
        {
            return Ok(OfferToDto(await _offerService.GetOffers()));
        }

        [HttpPost("WithFilter")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin+","+CustomRoles.User+","+CustomRoles.Recruiter)]
        public async Task<IActionResult> GetWithFilter(Filter filter)
        {
            var ret = OfferToDto((await _offerService.GetOffers()));
            if (filter.LocationIds != null && filter.LocationIds.Count != 0)
            {
                ret = ret.Where(q => q.Location != null && filter.LocationIds.Contains(q.Location.LocationId)).ToList();
            }

            if (filter.CategoryIds != null && filter.CategoryIds.Count != 0)
            {
                ret = ret.Where(q => q.Category != null && filter.CategoryIds.Contains(q.Category.CategoryId)).ToList();
            }

            if (filter.SalaryMin > 0)
            {
                ret = ret.Where(q => q.SalaryMin >= filter.SalaryMin).ToList();
            }

            return Ok(ret);
            //return Ok(OfferToDto((await _offerService.GetOffers())
            //    .Where(q => categoryIds.Contains(q.CategoryId))
            //    .ToList()));
        }

        [HttpGet("ById")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin+","+CustomRoles.User+","+CustomRoles.Recruiter)]
        public async Task<IActionResult> GetById(long offerId)
        {
            return Ok(OfferToDto(await _offerService.GetById(offerId)).First());
        }

        [HttpGet("ByUser")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin+","+CustomRoles.User+","+CustomRoles.Recruiter)]
        public async Task<IActionResult> GetByUser(long userId)
        {
            return Ok(OfferToDto(await _offerService.GetByUser(userId)));
        }

        [HttpPost("Create")]
        [AllowAnonymous] //[Authorize(Roles = CustomRoles.Admin)]
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
                User = new UserSimpleDto
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
                    User = new UserSimpleDto
                    {
                        UserId = r.User!.UserId, UserName = r.User.UserName,
                        Role = new RoleDto {RoleId = r.User.Role.RoleId, Name = r.User.Role.Name}
                    }
                }).ToList()
            }).ToList();
        }
    }
}