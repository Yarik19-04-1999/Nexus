using AutoFixture;
using FluentAssertions;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Tests.Utils;

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
        var ct = TestContext.Current.CancellationToken;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var rates = _fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>();

        _innerMock
            .Setup(x => x.GetRates(
                It.Is<IReadOnlyList<DateOnly>>(l => l.Count == 1 && l[0] == today),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Dictionary<DateOnly, IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>
            {
                [today] = rates
            });

        var result1 = await _provider.GetRates([today], ct);
        var result2 = await _provider.GetRates([today], ct);
        var result3 = await _provider.GetRates([today], ct);

        result1[today].Should().BeEquivalentTo(rates);
        result2[today].Should().BeEquivalentTo(rates);
        result3[today].Should().BeEquivalentTo(rates);

        _innerMock.Verify(x => x.GetRates(
            It.Is<IReadOnlyList<DateOnly>>(l => l.Count == 1 && l[0] == today),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetRates_ForDifferentDates_CallsInnerForEachDate()
    {
        var ct = TestContext.Current.CancellationToken;
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var yesterday = today.AddDays(-1);

        _innerMock
            .Setup(x => x.GetRates(It.IsAny<IReadOnlyList<DateOnly>>(), It.IsAny<CancellationToken>()))
            .Returns((IReadOnlyList<DateOnly> dates, CancellationToken _) =>
                Task.FromResult<IReadOnlyDictionary<DateOnly, IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>>(
                    dates.ToDictionary(
                        d => d,
                        _ => (IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>)_fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>())));

        await _provider.GetRates([today], ct);
        await _provider.GetRates([yesterday], ct);

        _innerMock.Verify(x => x.GetRates(
            It.Is<IReadOnlyList<DateOnly>>(l => l.Count == 1 && l[0] == today),
            It.IsAny<CancellationToken>()), Times.Once);
        _innerMock.Verify(x => x.GetRates(
            It.Is<IReadOnlyList<DateOnly>>(l => l.Count == 1 && l[0] == yesterday),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _factory.Dispose();
}
