using Information.Application.Enums;

namespace Information.Application.Models;

public class HourlyWeather
{
    public DateTime Time { get; init; }
    public double Temperature { get; init; }
    public double ApparentTemperature { get; init; }
    public int PrecipitationProbability { get; init; }
    public WeatherCode WeatherCode { get; init; }
    public double WindSpeed { get; init; }
}
