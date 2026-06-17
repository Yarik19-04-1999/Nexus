using FluentAssertions;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Application.Models.Options;
using Information.Application.Services;
using Information.Infrastructure.Decorators;
using Information.Integration.Tests.TestDoubles;
using Microsoft.Extensions.Options;
using Moq;

namespace Information.Integration.Tests.Caching;

public class CachingEpicGamesProviderTests
{
    private readonly Mock<IEpicGamesProvider> _innerMock;
    private readonly InMemoryCacheService _cacheService;
    private readonly CachingEpicGamesProvider _provider;

    public CachingEpicGamesProviderTests()
    {
        _innerMock = new Mock<IEpicGamesProvider>();
        _cacheService = new InMemoryCacheService();
        var cacheKeyProvider = new CacheKeyProvider();
        var options = Options.Create(new EpicGamesCacheOptions { CacheExpiration = TimeSpan.FromMinutes(10) });

        _provider = new CachingEpicGamesProvider(_innerMock.Object, _cacheService, cacheKeyProvider, options);
    }

    [Fact]
    public async Task GetFreeGames_CalledMultipleTimes_OnlyCallsInnerOnce()
    {
        var games = new List<EpicGame>
        {
            new()
            {
                Title = "Free Game",
                Description = "A free game",
                ImageUrl = "https://example.com/img.jpg",
                FreeUntil = DateTimeOffset.UtcNow.AddDays(7),
                StoreUrl = "https://store.epicgames.com/en-US/p/free-game",
            },
        };

        _innerMock
            .Setup(x => x.GetFreeGames(It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        var result1 = await _provider.GetFreeGames();
        var result2 = await _provider.GetFreeGames();
        var result3 = await _provider.GetFreeGames();

        result1.Should().BeEquivalentTo(games);
        result2.Should().BeEquivalentTo(games);
        result3.Should().BeEquivalentTo(games);

        _innerMock.Verify(x => x.GetFreeGames(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFreeGames_WhenCacheMiss_CallsInnerAndCachesResult()
    {
        _innerMock
            .Setup(x => x.GetFreeGames(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<EpicGame>());

        await _provider.GetFreeGames();

        _innerMock.Verify(x => x.GetFreeGames(It.IsAny<CancellationToken>()), Times.Once);
    }
}
