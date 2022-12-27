using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class LocationService : ModelServiceBase
{
    private readonly WorkItDbContext _context;

    public LocationService(WorkItDbContext context)
    {
        _context = context;
    }

    public async Task<Location> Create(string name)
    {
        var ret = new Location {LocationName = name};
        _context.Add(ret);
        await _context.SaveChangesAsync();

        return ret;
    }

    public async Task<List<Location>> GetLocations()
    {
        if (_context.Locations != null)
            return await _context.Locations.ToListAsync();
        return new List<Location>();
    }
}