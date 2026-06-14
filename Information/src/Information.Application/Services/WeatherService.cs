using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

namespace Information.Application.Services;

public class WeatherService : IWeatherService
{
    private readonly IWeatherProvider _weatherProvider;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyProvider _cacheKeyProvider;
    private readonly WeatherCacheOptions _cacheOptions;

    public WeatherService(IWeatherProvider weatherProvider, ICacheService cacheService, ICacheKeyProvider cacheKeyProvider, IOptions<WeatherCacheOptions> cacheOptions)
    {
        _weatherProvider = weatherProvider;
        _cacheService = cacheService;
        _cacheKeyProvider = cacheKeyProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<Result<CityWeather>> GetWeather(WeatherCity city, CancellationToken cancellationToken = default) =>
        await _cacheService.GetOrCreate(_cacheKeyProvider.GetWeatherKey(city), ct => _weatherProvider.GetWeather(city, ct), _cacheOptions.CacheExpiration, cancellationToken);
}
