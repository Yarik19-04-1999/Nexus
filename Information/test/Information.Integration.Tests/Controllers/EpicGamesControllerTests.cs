using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Integration.Tests.Utils;

namespace Information.Integration.Tests.Controllers;

public class EpicGamesControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetFreeGames_ReturnsOk_WithMappedGames(CancellationToken cancellationToken)
    {
        var games = _fixture.Create<List<EpicGame>>();

        var mock = new Mock<IGetEpicFreeGamesUseCase>();
        mock.Setup(x => x.Execute(GetEpicFreeGamesInput.Instance, It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/epicgames", cancellationToken);

        response.ShouldBeOk();
        var body = await response.ReadJsonResponse<GetEpicFreeGamesResponse>(cancellationToken);
        body.Should().NotBeNull();
        body!.Games.Should().HaveCount(games.Count);
        for (var i = 0; i < games.Count; i++)
        {
            body.Games[i].Title.Should().Be(games[i].Title);
            body.Games[i].Description.Should().Be(games[i].Description);
            body.Games[i].StoreUrl.Should().Be(games[i].StoreUrl);
            body.Games[i].FreeUntil.Should().Be(games[i].FreeUntil);
        }
        mock.Verify(x => x.Execute(GetEpicFreeGamesInput.Instance, It.IsAny<CancellationToken>()), Times.Once);
    }
}
