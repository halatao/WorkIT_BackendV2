using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WorkIT_Backend.Services;

namespace WorkIT_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("All")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _categoryService.GetCategory());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCategory(string name)
        {
            return Ok(await _categoryService.Create(name));
        }
    }
}