using FluentAssertions;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Infrastructure.Enums;
using Information.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests for all registered weather provider types.
/// Parametrized via Enum.GetValues — adding a new WeatherProviderType automatically adds coverage.
/// </summary>
public class WeatherProviderTests
{
    public static TheoryData<WeatherProviderType> AllProviderTypes { get; } =
        new(Enum.GetValues<WeatherProviderType>());

    public static TheoryData<WeatherCity> AllCities { get; } =
        new(Enum.GetValues<WeatherCity>());

    private static WebApplicationFactory<Program> CreateFactory(WeatherProviderType providerType) =>
        new InformationWebApplicationFactory()
            .WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, config) =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [$"{ConfigurationConstants.WeatherSection}:ProviderType"] = providerType.ToString()
                })));

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetHourlyForecast_ForKharkiv_Returns24Hours(WeatherProviderType providerType)
    {
        using var factory = CreateFactory(providerType);
        var provider = factory.Services.GetRequiredService<IWeatherProvider>();

        var forecast = await provider.GetHourlyForecast(WeatherCity.Kharkiv);

        forecast.Should().HaveCount(24);
        forecast.Should().AllSatisfy(h =>
        {
            h.Temperature.Should().BeInRange(-60, 60);
            h.WindSpeed.Should().BeGreaterThanOrEqualTo(0);
            h.PrecipitationProbability.Should().BeInRange(0, 100);
        });
    }

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetDailyForecast_ForKyiv_Returns5Days(WeatherProviderType providerType)
    {
        using var factory = CreateFactory(providerType);
        var provider = factory.Services.GetRequiredService<IWeatherProvider>();

        var forecast = await provider.GetDailyForecast(WeatherCity.Kyiv);

        forecast.Should().HaveCount(5);
        forecast.Should().AllSatisfy(d =>
        {
            d.MaxTemperature.Should().BeGreaterThanOrEqualTo(d.MinTemperature);
            d.MaxWindSpeed.Should().BeGreaterThanOrEqualTo(0);
            d.PrecipitationProbability.Should().BeInRange(0, 100);
        });
    }

    [Theory]
    [MemberData(nameof(AllCities))]
    public async Task GetHourlyForecast_ForAllCities_ReturnsForecast(WeatherCity city)
    {
        using var factory = CreateFactory(WeatherProviderType.OpenMeteo);
        var provider = factory.Services.GetRequiredService<IWeatherProvider>();

        var forecast = await provider.GetHourlyForecast(city);

        forecast.Should().NotBeEmpty();
    }
}
