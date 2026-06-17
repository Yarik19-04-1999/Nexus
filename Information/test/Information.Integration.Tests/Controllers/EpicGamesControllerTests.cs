using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Controllers;

public class EpicGamesControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = new();

    [Fact]
    public async Task GetFreeGames_ReturnsOk_WithMappedGames()
    {
        var games = _fixture.Create<List<EpicGame>>();

        var mock = new Mock<IGetEpicFreeGamesUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<GetEpicFreeGamesInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(games);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/epicgames");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetEpicFreeGamesResponse>();
        body.Should().NotBeNull();
        body!.Games.Should().HaveCount(games.Count);
        mock.Verify(x => x.Execute(
            It.IsAny<GetEpicFreeGamesInput>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
