using FluentAssertions;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Sieve.Models;

namespace Lore.Integration.Tests.UseCases;

public class GetMoviesUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_WithUniverseIdFilter_ReturnsOnlyMoviesForThatUniverse()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();
        var linked = await db.SeedMovie(m => m.UniverseId = universe.Id);
        var unlinked = await db.SeedMovie();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetMoviesUseCase>();

        var input = new GetMoviesInput(new SieveModel { Filters = $"universeId=={universe.Id}", PageSize = 100 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Items.Should().Contain(m => m.Id == linked.Id);
        result.Data.Items.Should().NotContain(m => m.Id == unlinked.Id);
    }

    [Fact]
    public async Task Execute_WithNoFilter_ReturnsAllMovies()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var m1 = await db.SeedMovie();
        var m2 = await db.SeedMovie();
        var m3 = await db.SeedMovie();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetMoviesUseCase>();

        var input = new GetMoviesInput(new SieveModel { PageSize = 100 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Items.Should().Contain(m => m.Id == m1.Id);
        result.Data.Items.Should().Contain(m => m.Id == m2.Id);
        result.Data.Items.Should().Contain(m => m.Id == m3.Id);
    }

    [Fact]
    public async Task Execute_WithPagination_ReturnsTotalCountCorrectly()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        await db.SeedMovie();
        await db.SeedMovie();
        await db.SeedMovie();
        await db.SeedMovie();
        await db.SeedMovie();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetMoviesUseCase>();

        var input = new GetMoviesInput(new SieveModel { Page = 1, PageSize = 2 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Items.Should().HaveCount(2);
        result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(5);
        result.Data.Page.Should().Be(1);
        result.Data.PageSize.Should().Be(2);
    }
}
