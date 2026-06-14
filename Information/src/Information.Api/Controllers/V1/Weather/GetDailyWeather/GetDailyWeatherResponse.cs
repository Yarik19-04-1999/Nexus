using Information.Api.Controllers.V1.Weather.GetDailyWeather.Dtos;
using Information.Application.Enums;

namespace Information.Api.Controllers.V1.Weather.GetDailyWeather;

public record GetDailyWeatherResponse(
    WeatherCity City,
    IReadOnlyList<DailyWeatherDto> Daily);
