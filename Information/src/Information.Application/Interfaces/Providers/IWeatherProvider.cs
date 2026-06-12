using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IWeatherProvider
{
    Task<Result<WeatherInfo>> GetWeather(string city, CancellationToken cancellationToken = default);
}
