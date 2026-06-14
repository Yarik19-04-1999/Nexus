using Information.Application.Enums;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IWeatherProvider
{
    Task<Result<CityWeather>> GetWeather(WeatherCity city, CancellationToken cancellationToken = default);
}
