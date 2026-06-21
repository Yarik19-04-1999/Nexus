using FluentAssertions;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class CreateUniverseUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_SavesUniverseToDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var name = $"{TestData.StringValue} {Guid.NewGuid()}";
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateUniverseUseCase>();

        var result = await useCase.Execute(new CreateUniverseInput(name, TestData.ChangedStringValue, false, TestData.IntValue), ct);

        result.HasError.Should().BeFalse();
        result.Data!.Id.Should().BeGreaterThan(0);
        result.Data.Name.Should().Be(name);
        result.Data.Description.Should().Be(TestData.ChangedStringValue);
        result.Data.IsHidden.Should().BeFalse();
        result.Data.ListNo.Should().Be(TestData.IntValue);

        var fromDb = await db.Context.Universes.FindAsync([result.Data.Id], TestContext.Current.CancellationToken);
        fromDb.Should().NotBeNull();
        fromDb!.Name.Should().Be(name);
        fromDb.Description.Should().Be(TestData.ChangedStringValue);
        fromDb.IsHidden.Should().BeFalse();
        fromDb.ListNo.Should().Be(TestData.IntValue);
        fromDb.CreatedAt.Should().NotBe(default);
        fromDb.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WithNullDescription_SavesNullToDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateUniverseUseCase>();

        var result = await useCase.Execute(new CreateUniverseInput($"{TestData.StringValue} {Guid.NewGuid()}", null, false, 0), ct);

        result.HasError.Should().BeFalse();
        var fromDb = await db.Context.Universes.FindAsync([result.Data!.Id], TestContext.Current.CancellationToken);
        fromDb!.Description.Should().BeNull();
    }

    [Fact]
    public async Task Execute_WithIsHidden_SavesIsHiddenToDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateUniverseUseCase>();

        var result = await useCase.Execute(new CreateUniverseInput($"{TestData.StringValue} {Guid.NewGuid()}", null, true, 0), ct);

        result.HasError.Should().BeFalse();
        var fromDb = await db.Context.Universes.FindAsync([result.Data!.Id], TestContext.Current.CancellationToken);
        fromDb!.IsHidden.Should().BeTrue();
    }

    [Fact]
    public async Task Execute_WhenUniverseWithSameNameExists_ReturnsAlreadyExistsError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var name = $"Duplicate universe {Guid.NewGuid()}";
        await db.SeedUniverse(u => u.Name = name);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateUniverseUseCase>();

        var result = await useCase.Execute(new CreateUniverseInput(name, null, false, 0), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(LoreErrorCodes.AlreadyExists);
    }
}
