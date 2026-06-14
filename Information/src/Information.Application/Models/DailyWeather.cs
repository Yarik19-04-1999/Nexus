using Information.Application.Enums;

namespace Information.Application.Models;

public class DailyWeather
{
    public DateOnly Date { get; init; }
    public double MaxTemperature { get; init; }
    public double MinTemperature { get; init; }
    public double PrecipitationSum { get; init; }
    public int PrecipitationProbability { get; init; }
    public WeatherCode WeatherCode { get; init; }
    public double MaxWindSpeed { get; init; }
}
