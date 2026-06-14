namespace Information.Application.Models.Options;

public class EpicGamesCacheOptions
{
    public TimeSpan CacheExpiration { get; init; } = TimeSpan.FromMinutes(10);
}
