using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Integration.Tests.Extensions;

namespace Information.Integration.Tests.Controllers;

public class EpicGamesControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly InformationWebApplicationFactory _factory = new();

    [Fact]
    public async Task GetFreeGames_ReturnsOk_WithMappedGames()
    {
        // Arrange
        var games = new List<EpicGame>
        {
            new()
            {
                Title = "Test Game",
                Description = "A great free game",
                ImageUrl = "https://example.com/image.jpg",
                FreeUntil = DateTimeOffset.UtcNow.AddDays(7),
                StoreUrl = "https://store.epicgames.com/en-US/p/test-game",
            }
        };

        Mock<IGetEpicFreeGamesUseCase> mockUseCase = null!;
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
        {
            mockUseCase = s.ReplaceWithMock<IGetEpicFreeGamesUseCase>(mock =>
                mock.Setup(x => x.Execute(It.IsAny<GetEpicFreeGamesInput>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(games));
        })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/epicgames");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetEpicFreeGamesResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.Games.Should().HaveCount(1);
        body.Games[0].Title.Should().Be("Test Game");
        body.Games[0].Description.Should().Be("A great free game");
        mockUseCase.Verify(x => x.Execute(
            It.IsAny<GetEpicFreeGamesInput>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetFreeGames_WhenNoGamesAvailable_ReturnsOkWithEmptyList()
    {
        // Arrange
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.ReplaceWithMock<IGetEpicFreeGamesUseCase>(mock =>
                mock.Setup(x => x.Execute(It.IsAny<GetEpicFreeGamesInput>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new List<EpicGame>()))))
            .CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/epicgames");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetEpicFreeGamesResponse>(JsonOptions);
        body!.Games.Should().BeEmpty();
    }
}
