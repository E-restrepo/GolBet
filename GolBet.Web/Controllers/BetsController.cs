using GolBet.Services.DTOs;
using GolBet.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GolBet.Web.Controllers;

public class BetsController : Controller
{
    private readonly IBetService _betService;
    private const string SessionBalanceKey = "UserFutCoins";
    private const decimal InitialBalance = 1000m;

    public BetsController(IBetService betService)
    {
        _betService = betService;
    }

    private decimal GetCurrentBalance()
    {
        var balanceStr = HttpContext.Session.GetString(SessionBalanceKey);
        if (decimal.TryParse(balanceStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var balance) && balance > 0)
        {
            return balance;
        }

        SetBalance(InitialBalance);
        return InitialBalance;
    }

    private void SetBalance(decimal balance)
    {
        HttpContext.Session.SetString(SessionBalanceKey, balance.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    // GET: /Bets
    public async Task<IActionResult> Index()
    {
        ViewBag.Balance = GetCurrentBalance();
        var bets = await _betService.GetUserBetsAsync();
        return View(bets);
    }

    // POST: /Bets/Place
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Place(CreateBetDto dto, string? returnUrl = null)
    {
        var currentBalance = GetCurrentBalance();

        if (dto.Amount > currentBalance)
        {
            TempData["Error"] = $"Saldo insuficiente. Tienes {currentBalance:N0} FutCoins e intentaste apostar {dto.Amount:N0} FutCoins.";
            return Redirect(returnUrl ?? Url.Action("Index", "Matches") ?? "/Matches");
        }

        var (success, message, createdBet) = await _betService.PlaceBetAsync(dto);

        if (!success)
        {
            TempData["Error"] = message;
            return Redirect(returnUrl ?? Url.Action("Index", "Matches") ?? "/Matches");
        }

        // Deduct bet amount from session
        SetBalance(currentBalance - dto.Amount);

        TempData["Success"] = $"{message} Se descontaron {dto.Amount:N0} FutCoins. Posible retorno: {createdBet?.PotentialPayout:N0} FutCoins.";
        return RedirectToAction(nameof(Index));
    }

    // POST: /Bets/ResetCoins
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ResetCoins()
    {
        SetBalance(InitialBalance);
        TempData["Success"] = $"¡Saldo recargado exitosamente con {InitialBalance:N0} FutCoins de prueba!";
        return RedirectToAction(nameof(Index));
    }
}
