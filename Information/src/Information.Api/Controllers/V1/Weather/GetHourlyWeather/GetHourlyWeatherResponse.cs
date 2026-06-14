using Information.Api.Controllers.V1.Weather.GetWeather.Dtos;
using Information.Application.Enums;

namespace Information.Api.Controllers.V1.Weather.GetHourlyWeather;

public record GetHourlyWeatherResponse(
    WeatherCity City,
    IReadOnlyList<HourlyWeatherDto> Hourly);
