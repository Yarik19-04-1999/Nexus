using Information.Application.Enums;
using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IWeatherProvider
{
    Task<IReadOnlyList<HourlyWeather>> GetHourlyForecast(WeatherCity city, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<DailyWeather>> GetDailyForecast(WeatherCity city, CancellationToken cancellationToken = default);
}
