using Information.Application.Enums;

namespace Information.Application.Models;

public class ExchangeRateHistory
{
    public ExchangeCurrency Currency { get; init; }
    public ExchangeRate Current { get; init; } = default!;
    public ExchangeRate? Yesterday { get; init; }
    public ExchangeRate? WeekAgo { get; init; }
    public ExchangeRate? MonthAgo { get; init; }
    public ExchangeRate? YearAgo { get; init; }
}
