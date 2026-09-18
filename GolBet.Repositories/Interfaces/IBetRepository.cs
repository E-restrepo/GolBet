using GolBet.Entities;

namespace GolBet.Repositories.Interfaces;

public interface IBetRepository : IGenericRepository<Bet>
{
    Task<IEnumerable<Bet>> GetAllWithDetailsAsync();
    Task<IEnumerable<Bet>> GetByMatchIdAsync(int matchId);
}
