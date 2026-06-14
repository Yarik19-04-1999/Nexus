using Information.Application.Enums;

namespace Information.Api.Controllers.V1.Weather.GetHourlyWeather.Dtos;

public record HourlyWeatherDto(
    DateTime Time,
    double Temperature,
    double ApparentTemperature,
    int PrecipitationProbability,
    WeatherCode WeatherCode,
    double WindSpeed);
