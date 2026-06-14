using Information.Application.Enums;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Services;

public interface IWeatherService
{
    Task<Result<CityWeather>> GetWeather(WeatherCity city, CancellationToken cancellationToken = default);
}
