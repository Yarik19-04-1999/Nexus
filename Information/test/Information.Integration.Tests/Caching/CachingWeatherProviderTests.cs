using AutoFixture;
using FluentAssertions;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Caching;

public class CachingWeatherProviderTests : IDisposable
{
    private readonly Mock<IWeatherProvider> _innerMock = new();
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IWeatherProvider _provider;
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    public CachingWeatherProviderTests()
    {
        _factory = new InformationWebApplicationFactory()
            .WithWebHostBuilder(b => b.ConfigureServices(s =>
            {
                s.AddSingleton(_innerMock.Object);
                s.Decorate<IWeatherProvider, CachingWeatherProvider>();
            }));
        _provider = _factory.Services.GetRequiredService<IWeatherProvider>();
    }

    [Fact]
    public async Task GetHourlyForecast_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var hourly = _fixture.Create<List<HourlyWeather>>();

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
        var daily = _fixture.Create<List<DailyWeather>>();

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
            .ReturnsAsync(_fixture.Create<List<HourlyWeather>>());

        await _provider.GetHourlyForecast(WeatherCity.Kharkiv);
        await _provider.GetHourlyForecast(WeatherCity.Kyiv);

        _innerMock.Verify(x => x.GetHourlyForecast(WeatherCity.Kharkiv, It.IsAny<CancellationToken>()), Times.Once);
        _innerMock.Verify(x => x.GetHourlyForecast(WeatherCity.Kyiv, It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _factory.Dispose();
}
