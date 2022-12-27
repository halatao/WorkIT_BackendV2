using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class UserService : ModelServiceBase
{
    private readonly WorkItDbContext _context;
    private readonly SecurityService _securityService;
    private readonly RoleService _roleService;

    public UserService([FromServices] WorkItDbContext context, [FromServices] SecurityService securityService,
        [FromServices] RoleService roleService)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _securityService = securityService;
        _roleService = roleService;
    }

    public async Task<User> GetUserByCredentials(string username, string password)
    {
        EnsureNotNull(username, nameof(username));
        EnsureNotNull(password, nameof(password));

        var user = await _context.Users.Include(q => q.Role).FirstOrDefaultAsync(q => q.UserName == username.ToLower())
                   ?? throw CreateException($"User {username} does not exist.");
        if (!_securityService.VerifyPassword(password, user.PasswordHash))
            throw CreateException($"Credentials are not valid.");

        return user;
    }

    public async Task<User> Create(string username, string password, string role)
    {
        EnsureNotNull(username, nameof(username));
        EnsureNotNull(password, nameof(password));
        EnsureNotNull(role, nameof(role));

        username = username.ToLower();

        if (_context.Users.Any(q => q.UserName == username))
            throw CreateException($"User {username} already exists.", null);

        var hash = _securityService.HashPassword(password);
        var userRole = await _roleService.GetRoleByName(role);
        var ret = new User {UserName = username, PasswordHash = hash, Role = userRole};

        _context.Users.Add(ret);
        await _context.SaveChangesAsync();

        return ret;
    }

    public async Task<List<User>> GetUsers()
    {
        return await _context.Users.Include(q => q.Role).ToListAsync();
    }
}