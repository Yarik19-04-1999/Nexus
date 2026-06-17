using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Information.Integration.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Controllers;

public class EpicGamesControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetFreeGames_ReturnsOk_WithMappedGames()
    {
        var games = _fixture.Create<List<EpicGame>>();

        var mock = new Mock<IGetEpicFreeGamesUseCase>();
        mock.Setup(x => x.Execute(GetEpicFreeGamesInput.Instance, It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/epicgames");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetEpicFreeGamesResponse>();
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
