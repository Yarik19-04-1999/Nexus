namespace Information.Application.Models;

public class ExchangeRate
{
    public string Currency { get; init; } = default!;
    public decimal Buy { get; init; }
    public decimal Sell { get; init; }
    public string Source { get; init; } = default!;
    public DateTime FetchedAt { get; init; }
}
