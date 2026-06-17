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
/// Real integration tests for all registered exchange rate provider types.
/// Parametrized via Enum.GetValues — adding a new ExchangeRateProviderType automatically adds coverage.
/// </summary>
public class ExchangeRateProviderTests
{
    public static TheoryData<ExchangeRateProviderType> AllProviderTypes { get; } =
        new(Enum.GetValues<ExchangeRateProviderType>());

    private static WebApplicationFactory<Program> CreateFactory(ExchangeRateProviderType providerType) =>
        new InformationWebApplicationFactory()
            .WithWebHostBuilder(b => b.ConfigureAppConfiguration((_, config) =>
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    [$"{ConfigurationConstants.ExchangeRateSection}:ProviderType"] = providerType.ToString()
                })));

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForToday_ReturnsAllKnownCurrencies(ExchangeRateProviderType providerType)
    {
        using var factory = CreateFactory(providerType);
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var rates = await provider.GetRates(today);

        rates.Should().NotBeEmpty();
        foreach (var currency in Enum.GetValues<ExchangeCurrency>())
        {
            rates.Should().ContainKey(currency);
        }
    }

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForToday_ReturnsPositiveRates(ExchangeRateProviderType providerType)
    {
        using var factory = CreateFactory(providerType);
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var rates = await provider.GetRates(today);

        foreach (var rate in rates.Values)
        {
            rate.Rate.Should().BeGreaterThan(0);
            rate.Date.Should().Be(today);
        }
    }

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForYesterday_ReturnsDifferentOrEqualRates(ExchangeRateProviderType providerType)
    {
        using var factory = CreateFactory(providerType);
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        var todayRates = await provider.GetRates(today);
        var yesterdayRates = await provider.GetRates(yesterday);

        todayRates.Should().NotBeEmpty();
        yesterdayRates.Should().NotBeEmpty();
        yesterdayRates.Keys.Should().BeEquivalentTo(todayRates.Keys);
    }
}
