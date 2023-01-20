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
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController([FromServices] CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("All")]
        [Authorize(Roles = CustomRoles.Admin + "," + CustomRoles.User + "," + CustomRoles.Recruiter)]
        public async Task<IActionResult> GetCategories()
        {
            return Ok((await _categoryService.GetCategory()).Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                CategoryName = c.CategoryName
            }));
        }

        [HttpPost("Create")]
        [Authorize(Roles = CustomRoles.Admin)]
        public async Task<IActionResult> CreateCategory(string name)
        {
            return Ok(await _categoryService.Create(name));
        }
    }
}