using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Infrastructure.Decorators;

internal class CachingWeatherProvider : IWeatherProvider
{
    private readonly IWeatherProvider _inner;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyProvider _cacheKeyProvider;
    private readonly WeatherCacheOptions _cacheOptions;

    public CachingWeatherProvider(IWeatherProvider inner, ICacheService cacheService, ICacheKeyProvider cacheKeyProvider, IOptions<WeatherCacheOptions> cacheOptions)
    {
        _inner = inner;
        _cacheService = cacheService;
        _cacheKeyProvider = cacheKeyProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<IReadOnlyList<HourlyWeather>> GetHourlyForecast(WeatherCity city, CancellationToken cancellationToken = default) =>
        await _cacheService.GetOrCreate(_cacheKeyProvider.GetWeatherHourlyKey(city), ct => _inner.GetHourlyForecast(city, ct), _cacheOptions.HourlyCacheExpiration, cancellationToken);

    public async Task<IReadOnlyList<DailyWeather>> GetDailyForecast(WeatherCity city, CancellationToken cancellationToken = default) =>
        await _cacheService.GetOrCreate(_cacheKeyProvider.GetWeatherDailyKey(city), ct => _inner.GetDailyForecast(city, ct), _cacheOptions.DailyCacheExpiration, cancellationToken);
}
