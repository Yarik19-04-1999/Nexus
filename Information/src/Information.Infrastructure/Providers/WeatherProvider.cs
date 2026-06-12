using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Providers;

public class WeatherProvider : IWeatherProvider
{
    public Task<Result<WeatherInfo>> GetWeather(string city, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
