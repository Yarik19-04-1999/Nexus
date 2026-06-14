namespace Information.Application.Models.Options;

public class WeatherCacheOptions
{
    public TimeSpan HourlyCacheExpiration { get; init; }
    public TimeSpan DailyCacheExpiration { get; init; }
}
