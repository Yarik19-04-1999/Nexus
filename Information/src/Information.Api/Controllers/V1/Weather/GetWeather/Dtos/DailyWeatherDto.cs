using Information.Application.Enums;

namespace Information.Api.Controllers.V1.Weather.GetWeather.Dtos;

public record DailyWeatherDto(
    DateOnly Date,
    double MaxTemperature,
    double MinTemperature,
    double PrecipitationSum,
    int PrecipitationProbability,
    WeatherCode WeatherCode,
    double MaxWindSpeed);
