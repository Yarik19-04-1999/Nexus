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

public class CachingExchangeRateProviderTests
{
    private readonly Mock<IExchangeRateProvider> _innerMock;
    private readonly InMemoryCacheService _cacheService;
    private readonly CachingExchangeRateProvider _provider;

    public CachingExchangeRateProviderTests()
    {
        _innerMock = new Mock<IExchangeRateProvider>();
        _cacheService = new InMemoryCacheService();
        var cacheKeyProvider = new CacheKeyProvider();
        var options = Options.Create(new ExchangeRateCacheOptions { CacheExpiration = TimeSpan.FromHours(1) });

        _provider = new CachingExchangeRateProvider(_innerMock.Object, _cacheService, cacheKeyProvider, options);
    }

    [Fact]
    public async Task GetRates_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rates = new Dictionary<ExchangeCurrency, ExchangeRate>
        {
            [ExchangeCurrency.USD] = new() { Rate = 41.5m, Date = today },
        };

        _innerMock
            .Setup(x => x.GetRates(today, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var result1 = await _provider.GetRates(today);
        var result2 = await _provider.GetRates(today);
        var result3 = await _provider.GetRates(today);

        result1.Should().BeEquivalentTo(rates);
        result2.Should().BeEquivalentTo(rates);
        result3.Should().BeEquivalentTo(rates);

        _innerMock.Verify(x => x.GetRates(today, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetRates_ForDifferentDates_CallsInnerForEachDate()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        _innerMock
            .Setup(x => x.GetRates(It.IsAny<DateOnly>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<ExchangeCurrency, ExchangeRate>());

        await _provider.GetRates(today);
        await _provider.GetRates(yesterday);

        _innerMock.Verify(x => x.GetRates(today, It.IsAny<CancellationToken>()), Times.Once);
        _innerMock.Verify(x => x.GetRates(yesterday, It.IsAny<CancellationToken>()), Times.Once);
    }
}
