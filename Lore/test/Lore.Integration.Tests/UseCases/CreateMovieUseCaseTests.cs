using FluentAssertions;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class CreateMovieUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_SavesAllFieldsToDatabase()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateMovieUseCase>();

        var input = new CreateMovieInput(
            Title: TestData.StringValue,
            ReleaseYear: 2023,
            DurationMinutes: 150,
            ReviewText: TestData.ChangedStringValue,
            Score: 7.5m,
            ViewCount: 2,
            RewatchStatus: RewatchStatus.OptionalRewatch,
            UniverseId: universe.Id,
            ListNo: TestData.IntValue);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Id.Should().BeGreaterThan(0);
        result.Data.Title.Should().Be(TestData.StringValue);
        result.Data.ReleaseYear.Should().Be(2023);
        result.Data.DurationMinutes.Should().Be(150);
        result.Data.ReviewText.Should().Be(TestData.ChangedStringValue);
        result.Data.Score.Should().Be(7.5m);
        result.Data.ViewCount.Should().Be(2);
        result.Data.RewatchStatus.Should().Be(RewatchStatus.OptionalRewatch);
        result.Data.UniverseId.Should().Be(universe.Id);
        result.Data.ListNo.Should().Be(TestData.IntValue);

        var fromDb = await db.Context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == result.Data.Id, ct);
        fromDb.Should().NotBeNull();
        fromDb!.Title.Should().Be(TestData.StringValue);
        fromDb.ReleaseYear.Should().Be(2023);
        fromDb.DurationMinutes.Should().Be(150);
        fromDb.ReviewText.Should().Be(TestData.ChangedStringValue);
        fromDb.Score.Should().Be(7.5m);
        fromDb.ViewCount.Should().Be(2);
        fromDb.RewatchStatus.Should().Be(RewatchStatus.OptionalRewatch);
        fromDb.UniverseId.Should().Be(universe.Id);
        fromDb.ListNo.Should().Be(TestData.IntValue);
        fromDb.CreatedAt.Should().NotBe(default);
        fromDb.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WithNullScore_SavesNullScore()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateMovieUseCase>();

        var input = new CreateMovieInput(
            Title: TestData.StringValue,
            ReleaseYear: 2023,
            DurationMinutes: 90,
            ReviewText: null,
            Score: null,
            ViewCount: 1,
            RewatchStatus: RewatchStatus.MustRewatch,
            UniverseId: null,
            ListNo: 0);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        var fromDb = await db.Context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == result.Data!.Id, ct);
        fromDb!.Score.Should().BeNull();
    }

    [Fact]
    public async Task Execute_WithoutUniverse_SavesNullUniverseId()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateMovieUseCase>();

        var input = new CreateMovieInput(
            Title: TestData.StringValue,
            ReleaseYear: 2023,
            DurationMinutes: 90,
            ReviewText: null,
            Score: null,
            ViewCount: 1,
            RewatchStatus: RewatchStatus.MustRewatch,
            UniverseId: null,
            ListNo: 0);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        var fromDb = await db.Context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == result.Data!.Id, ct);
        fromDb!.UniverseId.Should().BeNull();
    }
}
