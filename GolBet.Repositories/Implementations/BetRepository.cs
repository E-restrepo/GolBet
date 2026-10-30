using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Data;
using GolBet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GolBet.Repositories.Implementations;

public class BetRepository : GenericRepository<Bet>, IBetRepository
{
    public BetRepository(AppDbContext context) : base(context) { }

    public async Task<bool> ExistsAsync(int matchId, string userId, BetPick pick)
    {
        return await _dbSet.AnyAsync(b =>
            b.MatchId == matchId &&
            b.UserId == userId &&
            b.Pick == pick &&
            b.IsActive);
    }

    public async Task<IEnumerable<Bet>> GetByUserAsync(string userId)
    {
        return await _dbSet
            .Where(b => b.UserId == userId && b.IsActive)
            .Include(b => b.Match)
                .ThenInclude(m => m.HomeTeam)
            .Include(b => b.Match)
                .ThenInclude(m => m.AwayTeam)
            .OrderByDescending(b => b.CreatedDate)
            .AsNoTracking()
            .ToListAsync();
    }

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
