using AutoFixture;
using FluentAssertions;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.Infrastructure;
using Nexus.Core.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Caching;

public class CachingEpicGamesProviderTests : IDisposable
{
    private readonly Mock<IEpicGamesProvider> _innerMock = new();
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IEpicGamesProvider _provider;
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    public CachingEpicGamesProviderTests()
    {
        _factory = new InformationWebApplicationFactory()
            .WithWebHostBuilder(b => b.ConfigureServices(s =>
            {
                s.AddSingleton(_innerMock.Object);
                s.Decorate<IEpicGamesProvider, CachingEpicGamesProvider>();
            }));
        _provider = _factory.Services.GetRequiredService<IEpicGamesProvider>();
    }

    [Fact]
    public async Task GetFreeGames_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var games = _fixture.Create<List<EpicGame>>();

        _innerMock
            .Setup(x => x.GetFreeGames(It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        var result1 = await _provider.GetFreeGames(TestContext.Current.CancellationToken);
        var result2 = await _provider.GetFreeGames(TestContext.Current.CancellationToken);
        var result3 = await _provider.GetFreeGames(TestContext.Current.CancellationToken);

        result1.Should().BeEquivalentTo(games);
        result2.Should().BeEquivalentTo(games);
        result3.Should().BeEquivalentTo(games);

        _innerMock.Verify(x => x.GetFreeGames(It.IsAny<CancellationToken>()), Times.Once);
    }

    public void Dispose() => _factory.Dispose();
}
