using FluentAssertions;
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
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateUniverseUseCase>();

        var input = new CreateUniverseInput(TestData.StringValue, TestData.ChangedStringValue, false, TestData.IntValue);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data!.Id.Should().BeGreaterThan(0);
        result.Data.Name.Should().Be(TestData.StringValue);
        result.Data.Description.Should().Be(TestData.ChangedStringValue);
        result.Data.IsHidden.Should().BeFalse();
        result.Data.ListNo.Should().Be(TestData.IntValue);

        var fromDb = await db.Context.Universes.FindAsync([result.Data.Id], TestContext.Current.CancellationToken);
        fromDb.Should().NotBeNull();
        fromDb!.Name.Should().Be(TestData.StringValue);
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

        var input = new CreateUniverseInput(TestData.StringValue, null, false, 0);

        var result = await useCase.Execute(input, ct);

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

        var input = new CreateUniverseInput(TestData.StringValue, null, true, 0);

        var result = await useCase.Execute(input, ct);

        var fromDb = await db.Context.Universes.FindAsync([result.Data!.Id], TestContext.Current.CancellationToken);
        fromDb!.IsHidden.Should().BeTrue();
    }
}
