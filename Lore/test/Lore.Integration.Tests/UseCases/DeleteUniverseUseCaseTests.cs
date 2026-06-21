using FluentAssertions;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class DeleteUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_RemovesUniverseFromDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteUniverseUseCase>();

        var result = await useCase.Execute(new DeleteUniverseInput(universe.Id), ct);

        result.HasError.Should().BeFalse();

        var fromDb = await db.Context.Universes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == universe.Id, ct);
        fromDb.Should().BeNull();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteUniverseUseCase>();

        var result = await useCase.Execute(new DeleteUniverseInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
