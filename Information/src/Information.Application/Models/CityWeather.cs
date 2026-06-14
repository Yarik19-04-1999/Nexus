using Information.Application.Enums;

namespace Information.Application.Models;

public class CityWeather
{
    public WeatherCity City { get; init; }
    public IReadOnlyList<HourlyWeather> Hourly { get; init; } = default!;
    public IReadOnlyList<DailyWeather> Daily { get; init; } = default!;
}
