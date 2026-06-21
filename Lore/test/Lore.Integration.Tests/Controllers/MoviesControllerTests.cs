using AutoFixture;
using FluentAssertions;
using Lore.Api.Controllers.V1.Movies.CreateMovie;
using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Lore.Api.Controllers.V1.Movies.GetMovies;
using Lore.Api.Controllers.V1.Movies.UpdateMovie;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Results;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Application.Core.Models;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;
using Nexus.Core.Tests.Utils;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Lore.Integration.Tests.Controllers;

public class MoviesControllerTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetAll_ReturnsOk_AndPassesSieveModelToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var pagedResult = new PagedResult<Movie>([], 0, 1, 20);

        Expression<Func<IGetMoviesUseCase, Task<Result<PagedResult<Movie>>>>> execute = x => x.Execute(
            It.IsAny<GetMoviesInput>(),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IGetMoviesUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<PagedResult<Movie>>.Success(pagedResult));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync("/api/v1/movies", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetMoviesResponse>(ct);
        body.Should().NotBeNull();
        body!.TotalCount.Should().Be(0);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsOk_AndPassesIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var movie = _fixture.Create<Movie>();

        Expression<Func<IGetMovieByIdUseCase, Task<Result<Movie>>>> execute = x => x.Execute(
            It.Is<GetMovieByIdInput>(i => i.Id == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IGetMovieByIdUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Movie>.Success(movie));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync($"/api/v1/movies/{TestData.IntValue}", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetMovieByIdResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(movie.Id);
        body.Title.Should().Be(movie.Title);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IGetMovieByIdUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<GetMovieByIdInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoreResultConstants.MovieNotFound(TestData.NonExistentIntValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync($"/api/v1/movies/{TestData.NonExistentIntValue}", ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Create_ReturnsCreated_AndMapsRequestToInput()
    {
        var ct = TestContext.Current.CancellationToken;
        var movie = _fixture.Create<Movie>();

        Expression<Func<ICreateMovieUseCase, Task<Result<Movie>>>> execute = x => x.Execute(
            It.Is<CreateMovieInput>(i =>
                i.Title == TestData.StringValue &&
                i.ReleaseYear == TestData.IntValue &&
                i.DurationMinutes == 90 &&
                i.ReviewText == null &&
                i.Score == null &&
                i.ViewCount == 1 &&
                i.RewatchStatus == RewatchStatus.MustRewatch &&
                i.UniverseId == null &&
                i.ListNo == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<ICreateMovieUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Movie>.Success(movie));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new
        {
            Title = TestData.StringValue,
            ReleaseYear = TestData.IntValue,
            DurationMinutes = 90,
            ReviewText = (string?)null,
            Score = (decimal?)null,
            ViewCount = 1,
            RewatchStatus = RewatchStatus.MustRewatch,
            UniverseId = (int?)null,
            ListNo = TestData.IntValue,
        };
        var response = await client.PostAsJsonAsync("/api/v1/movies", request, ct);

        response.ShouldBeCreated();
        var body = await response.ReadResponse<GetMovieByIdResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(movie.Id);
        body.Title.Should().Be(movie.Title);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Create_WithEmptyTitle_ReturnsBadRequest()
    {
        var ct = TestContext.Current.CancellationToken;
        var client = _factory.CreateClient();

        var request = new
        {
            Title = "",
            ReleaseYear = 2024,
            DurationMinutes = 90,
            ReviewText = (string?)null,
            Score = (decimal?)null,
            ViewCount = 1,
            RewatchStatus = RewatchStatus.MustRewatch,
            UniverseId = (int?)null,
            ListNo = 0,
        };
        var response = await client.PostAsJsonAsync("/api/v1/movies", request, ct);

        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task Update_ReturnsOk_AndPassesFieldsToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var movie = _fixture.Create<Movie>();

        Expression<Func<IUpdateMovieUseCase, Task<Result<Movie>>>> execute = x => x.Execute(
            It.Is<UpdateMovieInput>(i =>
                i.Id == TestData.IntValue &&
                i.Title == TestData.ChangedStringValue &&
                i.ReleaseYear == TestData.IntValue &&
                i.DurationMinutes == 90 &&
                i.ReviewText == TestData.StringValue &&
                i.Score == 8m &&
                i.ViewCount == 2 &&
                i.RewatchStatus == RewatchStatus.OptionalRewatch &&
                i.UniverseId == null &&
                i.ListNo == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IUpdateMovieUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Movie>.Success(movie));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new
        {
            Title = TestData.ChangedStringValue,
            ReleaseYear = TestData.IntValue,
            DurationMinutes = 90,
            ReviewText = TestData.StringValue,
            Score = (decimal?)8m,
            ViewCount = 2,
            RewatchStatus = RewatchStatus.OptionalRewatch,
            UniverseId = (int?)null,
            ListNo = TestData.IntValue,
        };
        var response = await client.PutAsJsonAsync($"/api/v1/movies/{TestData.IntValue}", request, ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetMovieByIdResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(movie.Id);
        body.Title.Should().Be(movie.Title);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IUpdateMovieUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<UpdateMovieInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoreResultConstants.MovieNotFound(int.MaxValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new
        {
            Title = TestData.StringValue,
            ReleaseYear = 2024,
            DurationMinutes = 90,
            ReviewText = (string?)null,
            Score = (decimal?)null,
            ViewCount = 1,
            RewatchStatus = RewatchStatus.MustRewatch,
            UniverseId = (int?)null,
            ListNo = 0,
        };
        var response = await client.PutAsJsonAsync($"/api/v1/movies/{int.MaxValue}", request, ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_AndPassesIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;

        Expression<Func<IDeleteMovieUseCase, Task<Result>>> execute = x => x.Execute(
            It.Is<DeleteMovieInput>(i => i.Id == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IDeleteMovieUseCase>();
        mock.Setup(execute).ReturnsAsync(Result.Success());

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.DeleteAsync($"/api/v1/movies/{TestData.IntValue}", ct);

        response.ShouldBeNoContent();
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IDeleteMovieUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<DeleteMovieInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoreResultConstants.MovieNotFound(TestData.NonExistentIntValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.DeleteAsync($"/api/v1/movies/{TestData.NonExistentIntValue}", ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Link_ReturnsOk_AndPassesMovieIdAndUniverseIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;

        Expression<Func<ILinkMovieToUniverseUseCase, Task<Result>>> execute = x => x.Execute(
            It.Is<LinkMovieToUniverseInput>(i =>
                i.MovieId == TestData.IntValue &&
                i.UniverseId == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<ILinkMovieToUniverseUseCase>();
        mock.Setup(execute).ReturnsAsync(Result.Success());

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { MovieId = TestData.IntValue, UniverseId = TestData.IntValue };
        var response = await client.PostAsJsonAsync("/api/v1/movies/link", request, ct);

        response.ShouldBeOk();
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Link_WhenMovieNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<ILinkMovieToUniverseUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<LinkMovieToUniverseInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(LoreResultConstants.MovieNotFound(TestData.NonExistentIntValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { MovieId = TestData.NonExistentIntValue, UniverseId = TestData.IntValue };
        var response = await client.PostAsJsonAsync("/api/v1/movies/link", request, ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Unlink_ReturnsOk_AndPassesMovieIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;

        Expression<Func<IUnlinkMovieFromUniverseUseCase, Task<Result>>> execute = x => x.Execute(
            It.Is<UnlinkMovieFromUniverseInput>(i => i.MovieId == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IUnlinkMovieFromUniverseUseCase>();
        mock.Setup(execute).ReturnsAsync(Result.Success());

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { MovieId = TestData.IntValue };
        var response = await client.PostAsJsonAsync("/api/v1/movies/unlink", request, ct);

        response.ShouldBeOk();
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Search_ReturnsOk_AndPassesQueryToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var searchResults = new List<SearchMovieResult>
        {
            new SearchMovieResult(1, "Test Movie", 2024),
        };

        Expression<Func<ISearchMoviesUseCase, Task<Result<IReadOnlyList<SearchMovieResult>>>>> execute = x => x.Execute(
            It.Is<SearchMoviesInput>(i => i.Query == TestData.StringValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<ISearchMoviesUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<IReadOnlyList<SearchMovieResult>>.Success(searchResults));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync($"/api/v1/movies/search?q={TestData.StringValue}", ct);

        response.ShouldBeOk();
        mock.Verify(execute, Times.Once);
    }
}
