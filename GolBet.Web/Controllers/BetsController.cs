using GolBet.Entities;
using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GolBet.Web.Controllers;

[Authorize(Roles = "Bettor,Better")]
public class BetsController : Controller
{
    private readonly IBetService _betService;
    private readonly UserManager<AppUser> _userManager;

    public BetsController(IBetService betService, UserManager<AppUser> userManager)
    {
        _betService = betService;
        _userManager = userManager;
    }

    // GET: /Bets/MyBets
    [HttpGet]
    public async Task<IActionResult> MyBets()
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        var bets = await _betService.GetMyBetsAsync(userId);
        return View(bets);
    }

    // GET: /Bets
    [HttpGet]
    public IActionResult Index() => RedirectToAction(nameof(MyBets));

    // POST: /Bets/Place
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Place(BetFormDto dto)
    {
        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Por favor ingresa un monto válido.";
            return RedirectToAction("Detail", "Matches", new { id = dto.MatchId });
        }

        try
        {
            await _betService.PlaceBetAsync(userId, dto);
            TempData["Success"] = $"¡Apuesta registrada! {dto.Amount:N0} FootCoins en juego. ¡Suerte!";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Error"] = ex.Message;
        }
        catch (Exception)
        {
            TempData["Error"] = "Ocurrió un error inesperado al procesar la apuesta.";
        }

        return RedirectToAction("Detail", "Matches", new { id = dto.MatchId });
    }
}
