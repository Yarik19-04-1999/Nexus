using FluentAssertions;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class UpdateUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_UpdatesFieldsInDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();
        var newName = $"{TestData.ChangedStringValue} {Guid.NewGuid()}";

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateUniverseUseCase>();

        var input = new UpdateUniverseInput(universe.Id, newName, TestData.StringValue, true, TestData.IntValue);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Name.Should().Be(newName);
        result.Data.Description.Should().Be(TestData.StringValue);
        result.Data.IsHidden.Should().BeTrue();
        result.Data.ListNo.Should().Be(TestData.IntValue);

        db.Context.Entry(universe).State = EntityState.Detached;
        var fromDb = await db.Context.Universes.FindAsync([universe.Id], TestContext.Current.CancellationToken);
        fromDb!.Name.Should().Be(newName);
        fromDb.Description.Should().Be(TestData.StringValue);
        fromDb.IsHidden.Should().BeTrue();
        fromDb.ListNo.Should().Be(TestData.IntValue);
    }

    [Fact]
    public async Task Execute_UpdatesUpdatedAt()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse();
        var originalUpdatedAt = universe.UpdatedAt;

        await Task.Delay(10, ct);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateUniverseUseCase>();

        await useCase.Execute(new UpdateUniverseInput(universe.Id, $"{TestData.ChangedStringValue} {Guid.NewGuid()}", null, false, 0), ct);

        db.Context.Entry(universe).State = EntityState.Detached;
        var fromDb = await db.Context.Universes.FindAsync([universe.Id], TestContext.Current.CancellationToken);
        fromDb!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateUniverseUseCase>();

        var result = await useCase.Execute(new UpdateUniverseInput(TestData.NonExistentIntValue, TestData.StringValue, null, false, 0), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }

    [Fact]
    public async Task Execute_WhenAnotherUniverseWithSameNameExists_ReturnsAlreadyExistsError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var name = $"Duplicate universe {Guid.NewGuid()}";
        await db.SeedUniverse(u => u.Name = name);
        var universe = await db.SeedUniverse();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateUniverseUseCase>();

        var result = await useCase.Execute(new UpdateUniverseInput(universe.Id, name, null, false, 0), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.AlreadyExists);
    }
}
