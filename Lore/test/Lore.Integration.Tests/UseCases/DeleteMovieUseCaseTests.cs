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

public class DeleteMovieUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_RemovesMovieFromDatabase()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteMovieUseCase>();

        var result = await useCase.Execute(new DeleteMovieInput(movie.Id), ct);

        result.HasError.Should().BeFalse();

        var fromDb = await db.Context.Movies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == movie.Id, ct);
        fromDb.Should().BeNull();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteMovieUseCase>();

        var result = await useCase.Execute(new DeleteMovieInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
