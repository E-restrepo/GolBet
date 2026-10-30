using AutoMapper;
using GolBet.Entities;
using GolBet.Entities.Enums;
using GolBet.Repositories.Interfaces;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace GolBet.Services.Implementations;

public class BetService : IBetService
{
    private readonly IBetRepository _betRepository;
    private readonly IMatchRepository _matchRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IMapper _mapper;

    public BetService(
        IBetRepository betRepository,
        IMatchRepository matchRepository,
        UserManager<AppUser> userManager,
        IMapper mapper)
    {
        _betRepository = betRepository;
        _matchRepository = matchRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task PlaceBetAsync(string userId, BetFormDto dto)
    {
        // Regla 1: El partido debe existir, estar activo, programado y a futuro
        var match = await _matchRepository.GetByIdAsync(dto.MatchId);
        if (match == null || !match.IsActive)
        {
            throw new InvalidOperationException("El partido no existe.");
        }

        if (match.Status != MatchStatus.Scheduled || match.Date <= DateTime.UtcNow)
        {
            throw new InvalidOperationException("Este partido ya no acepta apuestas.");
        }

        // Regla 3: No permitir la misma selección en el mismo partido por el mismo usuario
        var alreadyExists = await _betRepository.ExistsAsync(dto.MatchId, userId, dto.Pick);
        if (alreadyExists)
        {
            throw new InvalidOperationException("Ya tienes una apuesta con esta selección en este partido.");
        }

        // Regla 2: El monto no puede exceder el balance
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado.");
        }

        if (dto.Amount > user.Balance)
        {
            throw new InvalidOperationException($"Saldo insuficiente. Tienes {user.Balance:N0} FootCoins.");
        }

        // Regla 4: Congelar la cuota seleccionada
        decimal oddsAtPlacement = dto.Pick switch
        {
            BetPick.Home => match.HomeOdds,
            BetPick.Draw => match.DrawOdds,
            BetPick.Away => match.AwayOdds,
            _ => throw new InvalidOperationException("Selección inválida.")
        };

        // Descontar saldo y actualizar usuario
        user.Balance -= dto.Amount;
        await _userManager.UpdateAsync(user);

        // Crear la apuesta y guardarla
        var bet = new Bet
        {
            MatchId = dto.MatchId,
            UserId = userId,
            Pick = dto.Pick,
            Amount = dto.Amount,
            OddsAtPlacement = oddsAtPlacement,
            Status = BetStatus.Pending
        };

        await _betRepository.AddAsync(bet);
    }

    public async Task<IEnumerable<BetDto>> GetMyBetsAsync(string userId)
    {
        var bets = await _betRepository.GetByUserAsync(userId);
        return _mapper.Map<IEnumerable<BetDto>>(bets);
    }

    public async Task<IEnumerable<BetDto>> GetUserBetsAsync()
    {
        var bets = await _betRepository.GetAllWithDetailsAsync();
        return _mapper.Map<IEnumerable<BetDto>>(bets);
    }
}
