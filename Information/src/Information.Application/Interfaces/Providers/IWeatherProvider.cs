using Information.Application.Enums;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IWeatherProvider
{
    Task<Result<IReadOnlyList<HourlyWeather>>> GetHourlyForecast(WeatherCity city, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyList<DailyWeather>>> GetDailyForecast(WeatherCity city, CancellationToken cancellationToken = default);
}
