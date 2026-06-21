using Nexus.Application.Core.Constants;
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

public class UnlinkMovieFromUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_ClearsUniverseIdOnMovie()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();
        var movie = await db.SeedMovie(m => m.UniverseId = universe.Id);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUnlinkMovieFromUniverseUseCase>();

        var result = await useCase.Execute(new UnlinkMovieFromUniverseInput(movie.Id), ct);

        result.HasError.Should().BeFalse();

        db.Context.Entry(movie).State = EntityState.Detached;
        var fromDb = await db.Context.Movies.FindAsync([movie.Id], TestContext.Current.CancellationToken);
        fromDb!.UniverseId.Should().BeNull();
    }

    [Fact]
    public async Task Execute_WhenMovieNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUnlinkMovieFromUniverseUseCase>();

        var result = await useCase.Execute(new UnlinkMovieFromUniverseInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
