using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class RoleService : ModelServiceBase
{
    private readonly WorkItDbContext _context;
    private readonly SecurityService _securityService;


    public RoleService(WorkItDbContext context, SecurityService securityService)
    {
        _context = context;
        _securityService = securityService;
    }

    public async Task<Role> Create(string? name)
    {
        EnsureNotNull(name, nameof(name));

        name = name.ToLower();
        if (_context.Roles.Any(q => q.Name == name))
            throw CreateException($"Role {name} already exists.");

        var ret = new Role { Name = name };
        _context.Add(ret);
        await _context.SaveChangesAsync();
        return ret;
    }

    public async Task<List<Role>> GetRoles()
    {
        var roles = await _context.Roles.ToListAsync();
        return roles;
    }

    public async Task<Role> GetRoleByName(string? name)
    {
        var role = await _context.Roles.FirstAsync(q => q.Name == name) ??
                   throw CreateException($"Role {name} does not exist.");
        return role;
    }

    public async Task<Role> GetRoleById(long id)
    {
        var role = await _context.Roles.FirstAsync(q => q.RoleId == id) ??
                   throw CreateException($"Role {id} does not exist.");
        return role;
    }
}