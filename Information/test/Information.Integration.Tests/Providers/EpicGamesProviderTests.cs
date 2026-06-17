using FluentAssertions;
using Information.Application.Interfaces.Providers;
using Information.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Information.Integration.Tests.Providers;

/// <summary>
/// Real integration tests against the live Epic Games Store API.
/// These tests make actual HTTP calls and require network access.
/// </summary>
public class EpicGamesProviderTests : IDisposable
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly IEpicGamesProvider _provider;

    public EpicGamesProviderTests()
    {
        _provider = _factory.Services.GetRequiredService<IEpicGamesProvider>();
    }

    [Fact]
    public async Task GetFreeGames_WhenGamesAvailable_HaveRequiredFields()
    {
        var games = await _provider.GetFreeGames();

        games.Should().NotBeNull();
        foreach (var game in games)
        {
            game.Title.Should().NotBeNullOrWhiteSpace();
            game.FreeUntil.Should().NotBe(default);
        }
    }

    public void Dispose() => _factory.Dispose();
}
