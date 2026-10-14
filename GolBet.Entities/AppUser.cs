using Microsoft.AspNetCore.Identity;

namespace GolBet.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public ICollection<Bet> Bets { get; set; } = new List<Bet>();
}
