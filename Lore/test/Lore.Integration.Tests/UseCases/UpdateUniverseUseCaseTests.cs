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

public class UpdateUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_UpdatesFieldsInDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse(u =>
        {
            u.Name = TestData.StringValue;
            u.IsHidden = false;
            u.ListNo = 0;
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateUniverseUseCase>();

        var input = new UpdateUniverseInput(universe.Id, TestData.ChangedStringValue, TestData.StringValue, true, TestData.IntValue);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Name.Should().Be(TestData.ChangedStringValue);
        result.Data.Description.Should().Be(TestData.StringValue);
        result.Data.IsHidden.Should().BeTrue();
        result.Data.ListNo.Should().Be(TestData.IntValue);

        db.Context.Entry(universe).State = EntityState.Detached;
        var fromDb = await db.Context.Universes.FindAsync(universe.Id);
        fromDb!.Name.Should().Be(TestData.ChangedStringValue);
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

        await useCase.Execute(new UpdateUniverseInput(universe.Id, TestData.ChangedStringValue, null, false, 0), ct);

        db.Context.Entry(universe).State = EntityState.Detached;
        var fromDb = await db.Context.Universes.FindAsync(universe.Id);
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
}
