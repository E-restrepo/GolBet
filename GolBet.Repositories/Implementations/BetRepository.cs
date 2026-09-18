using GolBet.Entities;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class BetRepository : GenericRepository<Bet>, IBetRepository
{
    public BetRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Bet>> GetAllWithDetailsAsync()
    {
        return await _dbSet
            .Include(b => b.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(b => b.Match)
                .ThenInclude(m => m.AwayTeam)
            .Where(b => b.IsActive)
            .OrderByDescending(b => b.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Bet>> GetByMatchIdAsync(int matchId)
    {
        return await _dbSet
            .Where(b => b.MatchId == matchId && b.IsActive)
            .OrderByDescending(b => b.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }
}
