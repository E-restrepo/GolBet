namespace GolBet.Services.Helpers;

public static class DateTimeExtensions
{
    private static readonly TimeZoneInfo ColombiaZone =
        TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");

    /// <summary>Converts a UTC DateTime to Colombia local time (UTC-5, no DST).</summary>
    public static DateTime ToColombiaTime(this DateTime utcDate)
        => TimeZoneInfo.ConvertTimeFromUtc(utcDate, ColombiaZone);

    /// <summary>Converts Colombia local time to UTC DateTime.</summary>
    public static DateTime ToUtcFromColombia(this DateTime localDate)
        => TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(localDate, DateTimeKind.Unspecified), ColombiaZone);

    /// <summary>Alias for ToUtcFromColombia.</summary>
    public static DateTime ToUtc(this DateTime localDate)
        => ToUtcFromColombia(localDate);
}
