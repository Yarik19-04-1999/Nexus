namespace Information.Application.Models.Options;

public class ExchangeRateCacheOptions
{
    public TimeSpan CacheExpiration { get; init; } = TimeSpan.FromHours(1);
}
