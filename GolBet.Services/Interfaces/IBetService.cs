using GolBet.Services.DTOs;

namespace GolBet.Services.Interfaces;

public interface IBetService
{
    Task<IEnumerable<BetDto>> GetUserBetsAsync();
    Task<(bool Success, string Message, BetDto? Bet)> PlaceBetAsync(CreateBetDto dto);
}
