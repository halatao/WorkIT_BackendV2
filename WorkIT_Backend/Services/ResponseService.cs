using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.EntityFrameworkCore;
using WorkIT_Backend.Data;
using WorkIT_Backend.Model;

namespace WorkIT_Backend.Services;

public class ResponseService : ModelServiceBase
{
    private readonly WorkItDbContext _context;
    private readonly SecurityService _securityService;

    public ResponseService(SecurityService securityService, WorkItDbContext context)
    {
        _securityService = securityService;
        _context = context;
    }

    public async Task<Response> Create(long offerId, long userId, string responseText, string cv)
    {
        EnsureNotNull(responseText, nameof(responseText));
        EnsureNotNull(cv, nameof(responseText));

        var ret = new Response {OfferId = offerId, UserId = userId, ResponseText = responseText, CurriculumVitae = cv};
        _context.Add(ret);
        await _context.SaveChangesAsync();
        return ret;
    }

    public async Task<List<Response>> GetByOffer(long offerId)
    {
        return (await GetIncluded()).Where(q => q.OfferId == offerId).ToList();
    }

    public async Task<List<Response>> GetByUser(long userId)
    {
        return (await GetIncluded()).Where(q => q.UserId == userId).ToList();
    }

    private async Task<List<Response>> GetIncluded()
    {
        if (_context.Responses != null)
            return await _context.Responses.ToListAsync();
        return new List<Response>();
    }
}