namespace Information.Application.Models.Options;

public class WeatherCacheOptions
{
    public TimeSpan CacheExpiration { get; init; } = TimeSpan.FromHours(1);
}
