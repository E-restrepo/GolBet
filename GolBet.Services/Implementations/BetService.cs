using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;

namespace GolBet.Services.Implementations;

public class BetService : IBetService
{
    private readonly IBetRepository _betRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly IMapper _mapper;

    public BetService(IBetRepository betRepository, IMatchRepository matchRepository, IMapper mapper)
    {
        _betRepository = betRepository;
        _matchRepository = matchRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BetDto>> GetUserBetsAsync()
    {
        var bets = await _betRepository.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<BetDto>>(bets);
    }

    public async Task<(bool Success, string Message, BetDto? Bet)> PlaceBetAsync(CreateBetDto dto)
    {
        if (dto.Amount < 10)
        {
            return (false, "El monto mínimo para apostar es de 10 FutCoins.", null);
        }

        var match = await _matchRepository.GetByIdWithDetailsAsync(dto.MatchId);
        if (match == null || !match.IsActive)
        {
            return (false, "El partido seleccionado no existe o no está disponible.", null);
        }

        if (match.Status != MatchStatus.Scheduled)
        {
            return (false, "Las apuestas para este partido ya están cerradas.", null);
        }

        decimal odds = dto.Pick switch
        {
            BetPick.Home => match.HomeOdds,
            BetPick.Draw => match.DrawOdds,
            BetPick.Away => match.AwayOdds,
            _ => 0m
        };

        if (odds <= 0)
        {
            return (false, "La cuota seleccionada no es válida.", null);
        }

        var bet = new Bet
        {
            MatchId = dto.MatchId,
            Pick = dto.Pick,
            Amount = dto.Amount,
            OddsAtPlacement = odds,
            Status = BetStatus.Pending
        };

        var created = await _betRepository.AddAsync(bet);
        created.Match = match;

        var resultDto = _mapper.Map<BetDto>(created);
        return (true, "¡Apuesta registrada con éxito!", resultDto);
    }
}
