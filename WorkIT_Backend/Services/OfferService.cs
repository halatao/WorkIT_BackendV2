using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Api;
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
        return await GetIncluded().Where(q => q.CategoryId == categoryId).ToListAsync();
    }

    public async Task<List<Offer>> GetByUser(long userId)
    {
        return await GetIncluded().Where(q => q.UserId == userId).ToListAsync();
    }

    public async Task<List<Offer>> GetByMinSalary(double salaryMin)
    {
        return await GetIncluded().Where(q => q.SalaryMin >= salaryMin).ToListAsync();
    }

    public async Task<List<Offer>> GetById(long offerId)
    {
        return await GetIncluded().Where(q => q.OfferId == offerId).ToListAsync();
    }

    public async Task<List<Offer>> GetOffers()
    {
        return await GetIncluded().ToListAsync();
    }

    public async Task<List<Offer>> GetFiltered(Filter filter)
    {
        if (_context.Offers == null)
            return new List<Offer>();
        var context = _context.Offers.Include(u => u.User)
            .ThenInclude(r => r!.Role)
            .Include(c => c.Category)
            .Include(l => l.Location).AsQueryable();

        if (filter.LocationIds != null && filter.LocationIds.Count != 0)
            context = context.Where(q => q.Location != null && filter.LocationIds.Contains(q.Location.LocationId));

        if (filter.CategoryIds != null && filter.CategoryIds.Count != 0)
            context = context.Where(q => q.Category != null && filter.CategoryIds.Contains(q.Category.CategoryId));

        if (filter.SalaryMin > 0)
            context = context.Where(q => q.SalaryMin >= filter.SalaryMin);

        if (!string.IsNullOrEmpty(filter.Search))
            context = context.Where(q =>
                q.OfferName != null && q.OfferName.ToLower().Contains(filter.Search.ToLower()));

        if (!(filter.Created.Date.Day == DateTime.Today.Day && filter.Created.Month == DateTime.Today.Month &&
              filter.Created.Year == DateTime.Today.Year))
            context = context.Where(q => q.Created <= filter.Created);

        return await context.ToListAsync();
    }

    private IQueryable<Offer> GetIncluded()
    {
        return ((_context.Offers ?? throw new InvalidOperationException()))
            .Include(u => u.User)
            .ThenInclude(r => r!.Role)
            .Include(c => c.Category)
            .Include(l => l.Location)
            .AsQueryable();
    }

    public List<Offer> OrderOffers(List<Offer> offers, string orderBy)
    {
        return orderBy switch
        {
            "date" => offers.OrderByDescending(o => o.Created).ToList(),
            "salary" => offers.OrderByDescending(o => (o.SalaryMin + o.SalaryMax) / 2).ToList(),
            "headline" => offers.OrderBy(o => o.OfferName).ToList(),
            _ => offers
        };
    }
}