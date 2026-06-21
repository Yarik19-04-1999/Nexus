using FluentAssertions;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class LinkMovieToUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_SetsUniverseIdOnMovie()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie();
        var universe = await db.SeedUniverse();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ILinkMovieToUniverseUseCase>();

        var result = await useCase.Execute(new LinkMovieToUniverseInput(movie.Id, universe.Id), ct);

        result.HasError.Should().BeFalse();

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.UniverseId.Should().Be(universe.Id);
    }

    [Fact]
    public async Task Execute_WhenMovieNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ILinkMovieToUniverseUseCase>();

        var result = await useCase.Execute(new LinkMovieToUniverseInput(TestData.NonExistentIntValue, universe.Id), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.MovieNotFound);
    }

    [Fact]
    public async Task Execute_WhenUniverseNotFound_ReturnsUniverseNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ILinkMovieToUniverseUseCase>();

        var result = await useCase.Execute(new LinkMovieToUniverseInput(movie.Id, TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.UniverseNotFound);
    }
}
