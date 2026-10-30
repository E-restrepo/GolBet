using System.ComponentModel.DataAnnotations;
using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

public class BetFormDto
{
    [Required]
    public int MatchId { get; set; }

    [Required]
    public BetPick Pick { get; set; }

    [Range(1, 9999999, ErrorMessage = "El monto debe estar entre 1 y 9.999.999 FootCoins.")]
    public decimal Amount { get; set; }
}

public class CreateBetDto
{
    [Required]
    public int MatchId { get; set; }

    [Required]
    public BetPick Pick { get; set; }

    [Range(1, 9999999, ErrorMessage = "La apuesta debe ser de al menos 1 FootCoin.")]
    public decimal Amount { get; set; }
}

public class BetDto
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public string MatchHomeTeamName { get; set; } = null!;
    public string MatchAwayTeamName { get; set; } = null!;
    public string HomeTeamName { get => MatchHomeTeamName; set => MatchHomeTeamName = value; }
    public string AwayTeamName { get => MatchAwayTeamName; set => MatchAwayTeamName = value; }
    public string? HomeTeamCrestUrl { get; set; }
    public string? AwayTeamCrestUrl { get; set; }
    public BetPick Pick { get; set; }
    public decimal OddsAtPlacement { get; set; }
    public decimal Amount { get; set; }
    public decimal PotentialPayout { get; set; }
    public BetStatus Status { get; set; }
    public MatchStatus MatchStatus { get; set; }
    public DateTime MatchDate { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public DateTime CreatedDate { get; set; }
}
