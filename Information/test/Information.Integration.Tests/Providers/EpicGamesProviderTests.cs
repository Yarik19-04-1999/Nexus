using FluentAssertions;
using Information.Infrastructure.Providers.EpicGames;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests against the live Epic Games Store API.
/// These tests make actual HTTP calls and require network access.
/// </summary>
public class EpicGamesProviderTests
{
    private readonly EpicGamesProvider _provider;

    public EpicGamesProviderTests()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://store-site-backend-static.ak.epicgames.com") };
        _provider = new EpicGamesProvider(httpClient);
    }

    [Fact]
    public async Task GetFreeGames_ReturnsResult()
    {
        var games = await _provider.GetFreeGames();

        games.Should().NotBeNull();
    }

    [Fact]
    public async Task GetFreeGames_WhenGamesAvailable_HaveRequiredFields()
    {
        var games = await _provider.GetFreeGames();

        foreach (var game in games)
        {
            game.Title.Should().NotBeNullOrWhiteSpace();
            game.FreeUntil.Should().NotBe(default);
        }
    }
}
