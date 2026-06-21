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

public class IncrementMovieViewCountUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_IncrementsViewCountByOne()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie(m => m.ViewCount = 2);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IIncrementMovieViewCountUseCase>();

        var result = await useCase.Execute(new IncrementMovieViewCountInput(movie.Id), ct);

        result.HasError.Should().BeFalse();
        result.Data!.ViewCount.Should().Be(3);

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.ViewCount.Should().Be(3);
    }

    [Fact]
    public async Task Execute_UpdatesUpdatedAt()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie();
        var originalUpdatedAt = movie.UpdatedAt;

        await Task.Delay(10, ct);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IIncrementMovieViewCountUseCase>();

        await useCase.Execute(new IncrementMovieViewCountInput(movie.Id), ct);

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IIncrementMovieViewCountUseCase>();

        var result = await useCase.Execute(new IncrementMovieViewCountInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.MovieNotFound);
    }
}
