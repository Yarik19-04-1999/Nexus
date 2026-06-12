namespace Information.Application.Models;

public class WeatherInfo
{
    public string City { get; init; } = default!;
    public string Country { get; init; } = default!;
    public double TemperatureCelsius { get; init; }
    public double FeelsLikeCelsius { get; init; }
    public int HumidityPercent { get; init; }
    public string Description { get; init; } = default!;
    public DateTime FetchedAt { get; init; }
}
