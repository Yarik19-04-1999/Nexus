using FluentAssertions;
using Information.Application.Enums;
using Information.Infrastructure.Providers.Nbu;
using Information.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests against the live NBU (National Bank of Ukraine) API.
/// These tests make actual HTTP calls and require network access.
/// </summary>
public class NbuExchangeRateProviderTests : IDisposable
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly NbuExchangeRateProvider _provider;

    public NbuExchangeRateProviderTests()
    {
        _provider = _factory.Services.GetRequiredService<NbuExchangeRateProvider>();
    }

    [Fact]
    public async Task GetRates_ForToday_ReturnsAllKnownCurrencies()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var rates = await _provider.GetRates(today);

        rates.Should().NotBeEmpty();
        rates.Should().ContainKey(ExchangeCurrency.USD);
        rates.Should().ContainKey(ExchangeCurrency.EUR);
        rates.Should().ContainKey(ExchangeCurrency.GBP);
    }

    [Fact]
    public async Task GetRates_ForToday_ReturnsPositiveRates()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var rates = await _provider.GetRates(today);

        foreach (var rate in rates.Values)
        {
            rate.Rate.Should().BeGreaterThan(0);
            rate.Date.Should().Be(today);
        }
    }

    [Fact]
    public async Task GetRates_ForYesterday_ReturnsDifferentOrEqualRates()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        var todayRates = await _provider.GetRates(today);
        var yesterdayRates = await _provider.GetRates(yesterday);

        todayRates.Should().NotBeEmpty();
        yesterdayRates.Should().NotBeEmpty();
        yesterdayRates.Keys.Should().BeEquivalentTo(todayRates.Keys);
    }

    public void Dispose() => _factory.Dispose();
}
