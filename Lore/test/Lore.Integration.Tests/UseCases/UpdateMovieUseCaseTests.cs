using FluentAssertions;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class UpdateMovieUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_UpdatesAllFieldsInDatabase()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie(m =>
        {
            m.Title = TestData.StringValue;
            m.ReleaseYear = 2020;
            m.DurationMinutes = 100;
            m.Score = null;
            m.ViewCount = 1;
            m.RewatchStatus = RewatchStatus.MustRewatch;
            m.ListNo = 0;
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateMovieUseCase>();

        var input = new UpdateMovieInput(
            Id: movie.Id,
            Title: TestData.ChangedStringValue,
            ReleaseYear: 2023,
            DurationMinutes: 150,
            ReviewText: TestData.StringValue,
            Score: 9m,
            ViewCount: 3,
            RewatchStatus: RewatchStatus.NotWorthRewatching,
            UniverseId: null,
            ListNo: TestData.IntValue);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Title.Should().Be(TestData.ChangedStringValue);
        result.Data.ReleaseYear.Should().Be(2023);
        result.Data.DurationMinutes.Should().Be(150);
        result.Data.ReviewText.Should().Be(TestData.StringValue);
        result.Data.Score.Should().Be(9m);
        result.Data.ViewCount.Should().Be(3);
        result.Data.RewatchStatus.Should().Be(RewatchStatus.NotWorthRewatching);
        result.Data.ListNo.Should().Be(TestData.IntValue);

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.Title.Should().Be(TestData.ChangedStringValue);
        fromDb.ReleaseYear.Should().Be(2023);
        fromDb.DurationMinutes.Should().Be(150);
        fromDb.ReviewText.Should().Be(TestData.StringValue);
        fromDb.Score.Should().Be(9m);
        fromDb.ViewCount.Should().Be(3);
        fromDb.RewatchStatus.Should().Be(RewatchStatus.NotWorthRewatching);
        fromDb.ListNo.Should().Be(TestData.IntValue);
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
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateMovieUseCase>();

        await useCase.Execute(new UpdateMovieInput(
            Id: movie.Id,
            Title: TestData.ChangedStringValue,
            ReleaseYear: movie.ReleaseYear,
            DurationMinutes: movie.DurationMinutes,
            ReviewText: null,
            Score: null,
            ViewCount: 1,
            RewatchStatus: RewatchStatus.MustRewatch,
            UniverseId: null,
            ListNo: 0), ct);

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateMovieUseCase>();

        var result = await useCase.Execute(new UpdateMovieInput(
            Id: TestData.NonExistentIntValue,
            Title: TestData.StringValue,
            ReleaseYear: 2023,
            DurationMinutes: 90,
            ReviewText: null,
            Score: null,
            ViewCount: 1,
            RewatchStatus: RewatchStatus.MustRewatch,
            UniverseId: null,
            ListNo: 0), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.MovieNotFound);
    }
}
