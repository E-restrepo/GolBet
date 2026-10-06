using System.ComponentModel.DataAnnotations;
using GolBet.Entities.Enums;

namespace GolBet.Services.DTOs;

/// <summary>
/// Form DTO for creating and editing matches with validation rules.
/// </summary>
public class MatchFormDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El equipo local es obligatorio")]
    [Display(Name = "Equipo local")]
    public int HomeTeamId { get; set; }

    [Required(ErrorMessage = "El equipo visitante es obligatorio")]
    [Display(Name = "Equipo visitante")]
    public int AwayTeamId { get; set; }

    [Required(ErrorMessage = "La fecha es obligatoria")]
    [Display(Name = "Fecha y hora")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "La cuota local es obligatoria")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor a 1")]
    [Display(Name = "Cuota local (1)")]
    public decimal HomeOdds { get; set; }

    [Required(ErrorMessage = "La cuota de empate es obligatoria")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor a 1")]
    [Display(Name = "Cuota empate (X)")]
    public decimal DrawOdds { get; set; }

    [Required(ErrorMessage = "La cuota visitante es obligatoria")]
    [Range(1.01, 999.99, ErrorMessage = "La cuota debe ser mayor a 1")]
    [Display(Name = "Cuota visitante (2)")]
    public decimal AwayOdds { get; set; }

    [Display(Name = "Estado")]
    public MatchStatus Status { get; set; } = MatchStatus.Scheduled;

    [Display(Name = "Goles local")]
    [Range(0, 99, ErrorMessage = "Goles inválidos")]
    public int? HomeGoals { get; set; }

    [Display(Name = "Goles visitante")]
    [Range(0, 99, ErrorMessage = "Goles inválidos")]
    public int? AwayGoals { get; set; }
}
