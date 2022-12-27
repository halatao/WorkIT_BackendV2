using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class OfferService : ModelServiceBase
{
    private readonly WorkItDbContext _context;
    private readonly SecurityService _securityService;

    public OfferService(SecurityService securityService, WorkItDbContext context)
    {
        _securityService = securityService;
        _context = context;
    }

    public async Task<Offer> Create(string offerName, string offerDesc, long userId, long categoryId, long locationId,
        double salaryMin,
        double salaryMax)
    {
        EnsureNotNull(offerName, nameof(offerName));
        EnsureNotNull(offerDesc, nameof(offerDesc));

        var ret = new Offer
        {
            OfferName = offerName,
            OfferDescription = offerDesc,
            UserId = userId,
            CategoryId = categoryId,
            LocationId = locationId,
            SalaryMin = salaryMin,
            SalaryMax = salaryMax
        };
        _context.Add(ret);
        await _context.SaveChangesAsync();
        return ret;
    }

    public async Task<List<Offer>> GetByCategory(long categoryId)
    {
        return (await GetIncluded()).Where(q => q.CategoryId == categoryId).ToList();
    }

    public async Task<List<Offer>> GetByUser(long userId)
    {
        return (await GetIncluded()).Where(q => q.UserId == userId).ToList();
    }

    public async Task<List<Offer>> GetByMinSalary(double salaryMin)
    {
        return (await GetIncluded()).Where(q => q.SalaryMin >= salaryMin).ToList();
    }

    public async Task<List<Offer>> GetIncluded()
    {
        if (_context.Offers != null)
            return await _context.Offers
                .Include(u => u.User)
                .ThenInclude(r => r!.Role)
                .Include(c => c.Category)
                .Include(l => l.Location)
                .ToListAsync();
        return new List<Offer>();
    }
}