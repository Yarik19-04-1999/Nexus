using Information.Api.Controllers.V1.Weather.GetWeather.Dtos;
using Information.Application.Enums;

namespace Information.Api.Controllers.V1.Weather.GetWeather;

public record GetWeatherResponse(
    WeatherCity City,
    IReadOnlyList<HourlyWeatherDto> Hourly,
    IReadOnlyList<DailyWeatherDto> Daily);
