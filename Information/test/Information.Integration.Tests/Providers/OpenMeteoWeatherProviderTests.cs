using FluentAssertions;
using Information.Application.Enums;
using Information.Infrastructure.Providers.OpenMeteo;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests against the live Open-Meteo API.
/// These tests make actual HTTP calls and require network access.
/// </summary>
public class OpenMeteoWeatherProviderTests
{
    private readonly OpenMeteoWeatherProvider _provider;

    public OpenMeteoWeatherProviderTests()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://api.open-meteo.com") };
        _provider = new OpenMeteoWeatherProvider(httpClient);
    }

    [Fact]
    public async Task GetHourlyForecast_ForKharkiv_Returns24Hours()
    {
        var forecast = await _provider.GetHourlyForecast(WeatherCity.Kharkiv);

        forecast.Should().HaveCount(24);
        forecast.Should().AllSatisfy(h =>
        {
            h.Temperature.Should().BeInRange(-60, 60);
            h.WindSpeed.Should().BeGreaterThanOrEqualTo(0);
            h.PrecipitationProbability.Should().BeInRange(0, 100);
        });
    }

    [Fact]
    public async Task GetDailyForecast_ForKyiv_Returns5Days()
    {
        var forecast = await _provider.GetDailyForecast(WeatherCity.Kyiv);

        forecast.Should().HaveCount(5);
        forecast.Should().AllSatisfy(d =>
        {
            d.MaxTemperature.Should().BeGreaterThanOrEqualTo(d.MinTemperature);
            d.MaxWindSpeed.Should().BeGreaterThanOrEqualTo(0);
            d.PrecipitationProbability.Should().BeInRange(0, 100);
        });
    }

    [Theory]
    [InlineData(WeatherCity.Kharkiv)]
    [InlineData(WeatherCity.Kyiv)]
    [InlineData(WeatherCity.Odesa)]
    [InlineData(WeatherCity.Lviv)]
    public async Task GetHourlyForecast_ForAllCities_ReturnsForecast(WeatherCity city)
    {
        var forecast = await _provider.GetHourlyForecast(city);

        forecast.Should().NotBeEmpty();
    }
}
