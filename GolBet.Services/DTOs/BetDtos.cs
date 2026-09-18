using System.ComponentModel.DataAnnotations;
using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

public class CreateBetDto
{
    [Required]
    public int MatchId { get; set; }

    [Required]
    public BetPick Pick { get; set; }

    [Range(10, 100000, ErrorMessage = "La apuesta debe ser de al menos 10 FutCoins.")]
    public decimal Amount { get; set; }
}

public class BetDto
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public string HomeTeamName { get; set; } = null!;
    public string AwayTeamName { get; set; } = null!;
    public string? HomeTeamCrestUrl { get; set; }
    public string? AwayTeamCrestUrl { get; set; }
    public BetPick Pick { get; set; }
    public decimal OddsAtPlacement { get; set; }
    public decimal Amount { get; set; }
    public decimal PotentialPayout => Math.Round(Amount * OddsAtPlacement, 2);
    public BetStatus Status { get; set; }
    public MatchStatus MatchStatus { get; set; }
    public DateTime MatchDate { get; set; }
    public int? HomeGoals { get; set; }
    public int? AwayGoals { get; set; }
    public DateTime CreatedDate { get; set; }
}
