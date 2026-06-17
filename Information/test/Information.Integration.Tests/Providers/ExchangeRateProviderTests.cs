using FluentAssertions;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Infrastructure.Enums;
using Information.Integration.Tests.Infrastructure;
using Nexus.Core.Integration.Tests.Extensions;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests for all registered exchange rate provider types.
/// Parametrized via Enum.GetValues — adding a new ExchangeRateProviderType automatically adds coverage.
/// </summary>
public class ExchangeRateProviderTests
{
    public static TheoryData<ExchangeRateProviderType> AllProviderTypes { get; } =
        new(Enum.GetValues<ExchangeRateProviderType>());

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForToday_ReturnsAllKnownCurrencies(ExchangeRateProviderType providerType, CancellationToken cancellationToken)
    {
        using var factory = new InformationWebApplicationFactory()
            .WithConfiguration($"{ConfigurationConstants.ExchangeRateSection}:ProviderType", providerType.ToString());
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await provider.GetRates([today], cancellationToken);

        result.Should().ContainKey(today);
        var rates = result[today];
        rates.Should().NotBeEmpty();
        foreach (var currency in Enum.GetValues<ExchangeCurrency>())
        {
            rates.Should().ContainKey(currency);
        }
    }

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForToday_ReturnsPositiveRates(ExchangeRateProviderType providerType, CancellationToken cancellationToken)
    {
        using var factory = new InformationWebApplicationFactory()
            .WithConfiguration($"{ConfigurationConstants.ExchangeRateSection}:ProviderType", providerType.ToString());
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var result = await provider.GetRates([today], cancellationToken);

        result.Should().ContainKey(today);
        foreach (var rate in result[today].Values)
        {
            rate.Rate.Should().BeGreaterThan(0);
            rate.Date.Should().Be(today);
        }
    }

    [Theory]
    [MemberData(nameof(AllProviderTypes))]
    public async Task GetRates_ForMultipleDates_ReturnsBothDates(ExchangeRateProviderType providerType, CancellationToken cancellationToken)
    {
        using var factory = new InformationWebApplicationFactory()
            .WithConfiguration($"{ConfigurationConstants.ExchangeRateSection}:ProviderType", providerType.ToString());
        var provider = factory.Services.GetRequiredService<IExchangeRateProvider>();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        var result = await provider.GetRates([today, yesterday], cancellationToken);

        result.Should().ContainKey(today);
        result.Should().ContainKey(yesterday);
        result[today].Should().NotBeEmpty();
        result[yesterday].Should().NotBeEmpty();
        result[yesterday].Keys.Should().BeEquivalentTo(result[today].Keys);
    }
}
