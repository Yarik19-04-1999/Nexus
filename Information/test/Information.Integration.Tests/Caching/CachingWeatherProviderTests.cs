using FluentAssertions;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Application.Models.Options;
using Information.Application.Services;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.TestDoubles;
using Microsoft.Extensions.Options;
using Moq;

namespace Information.Integration.Tests.Caching;

public class CachingWeatherProviderTests
{
    private readonly Mock<IWeatherProvider> _innerMock;
    private readonly InMemoryCacheService _cacheService;
    private readonly CachingWeatherProvider _provider;

    public CachingWeatherProviderTests()
    {
        _innerMock = new Mock<IWeatherProvider>();
        _cacheService = new InMemoryCacheService();
        var cacheKeyProvider = new CacheKeyProvider();
        var options = Options.Create(new WeatherCacheOptions
        {
            HourlyCacheExpiration = TimeSpan.FromMinutes(10),
            DailyCacheExpiration = TimeSpan.FromHours(1),
        });

        _provider = new CachingWeatherProvider(_innerMock.Object, _cacheService, cacheKeyProvider, options);
    }

    [Fact]
    public async Task GetHourlyForecast_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var hourly = new List<HourlyWeather>
        {
            new() { Time = DateTime.UtcNow, Temperature = 22.0, WindSpeed = 5.0, PrecipitationProbability = 10, WeatherCode = WeatherCode.ClearSky, ApparentTemperature = 21.0 },
        };

        _innerMock
            .Setup(x => x.GetHourlyForecast(WeatherCity.Kharkiv, It.IsAny<CancellationToken>()))
            .ReturnsAsync(hourly);

        var result1 = await _provider.GetHourlyForecast(WeatherCity.Kharkiv);
        var result2 = await _provider.GetHourlyForecast(WeatherCity.Kharkiv);
        var result3 = await _provider.GetHourlyForecast(WeatherCity.Kharkiv);

        result1.Should().BeEquivalentTo(hourly);
        result2.Should().BeEquivalentTo(hourly);
        result3.Should().BeEquivalentTo(hourly);

        _innerMock.Verify(x => x.GetHourlyForecast(WeatherCity.Kharkiv, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDailyForecast_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var daily = new List<DailyWeather>
        {
            new() { Date = DateOnly.FromDateTime(DateTime.Today), MaxTemperature = 28.0, MinTemperature = 17.0, PrecipitationSum = 0.0, PrecipitationProbability = 5, WeatherCode = WeatherCode.ClearSky, MaxWindSpeed = 10.0 },
        };

        _innerMock
            .Setup(x => x.GetDailyForecast(WeatherCity.Kyiv, It.IsAny<CancellationToken>()))
            .ReturnsAsync(daily);

        var result1 = await _provider.GetDailyForecast(WeatherCity.Kyiv);
        var result2 = await _provider.GetDailyForecast(WeatherCity.Kyiv);

        result1.Should().BeEquivalentTo(daily);
        result2.Should().BeEquivalentTo(daily);

        _innerMock.Verify(x => x.GetDailyForecast(WeatherCity.Kyiv, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHourlyForecast_ForDifferentCities_CallsInnerForEachCity()
    {
        _innerMock
            .Setup(x => x.GetHourlyForecast(It.IsAny<WeatherCity>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<HourlyWeather>());

        await _provider.GetHourlyForecast(WeatherCity.Kharkiv);
        await _provider.GetHourlyForecast(WeatherCity.Kyiv);

        _innerMock.Verify(x => x.GetHourlyForecast(WeatherCity.Kharkiv, It.IsAny<CancellationToken>()), Times.Once);
        _innerMock.Verify(x => x.GetHourlyForecast(WeatherCity.Kyiv, It.IsAny<CancellationToken>()), Times.Once);
    }
}
