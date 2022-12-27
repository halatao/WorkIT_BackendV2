using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class CategoryService : ModelServiceBase
{
    private readonly WorkItDbContext _context;
    private readonly SecurityService _securityService;

    public CategoryService(WorkItDbContext context, SecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    public async Task<Category> Create(string name)
    {
        var ret = new Category {CategoryName = name};
        _context.Add(ret);
        await _context.SaveChangesAsync();
        return ret;
    }

    public async Task<List<Category>> GetCategory()
    {
        if (_context.Categories != null)
            return await _context.Categories.ToListAsync();
        return new List<Category>();
    }
}