using AutoFixture;
using FluentAssertions;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.Infrastructure;
using Information.Integration.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Caching;

public class CachingExchangeRateProviderTests : IDisposable
{
    private readonly Mock<IExchangeRateProvider> _innerMock = new();
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IExchangeRateProvider _provider;
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    public CachingExchangeRateProviderTests()
    {
        _factory = new InformationWebApplicationFactory()
            .WithWebHostBuilder(b => b.ConfigureServices(s =>
            {
                s.AddSingleton(_innerMock.Object);
                s.Decorate<IExchangeRateProvider, CachingExchangeRateProvider>();
            }));
        _provider = _factory.Services.GetRequiredService<IExchangeRateProvider>();
    }

    [Fact]
    public async Task GetRates_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rates = _fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>();

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
            .ReturnsAsync(_fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>());

        await _provider.GetRates(today);
        await _provider.GetRates(yesterday);

        _innerMock.Verify(x => x.GetRates(today, It.IsAny<CancellationToken>()), Times.Once);
        _innerMock.Verify(x => x.GetRates(yesterday, It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _factory.Dispose();
}
