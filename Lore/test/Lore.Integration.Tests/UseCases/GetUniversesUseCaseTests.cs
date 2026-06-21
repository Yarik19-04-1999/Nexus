using FluentAssertions;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Sieve.Models;

namespace Lore.Integration.Tests.UseCases;

public class GetUniversesUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_WithNameFilter_ReturnsMatchingUniverses()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var uniqueName = $"Filtered-{Guid.NewGuid()}";
        var match = await db.SeedUniverse(u => u.Name = uniqueName);
        var other = await db.SeedUniverse(u => u.Name = $"Other-{Guid.NewGuid()}");

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUniversesUseCase>();

        var input = new GetUniversesInput(new SieveModel { Filters = $"name=={uniqueName}", PageSize = 100 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(u => u.Id == match.Id);
        result.Data.Items.Should().NotContain(u => u.Id == other.Id);
    }

    [Fact]
    public async Task Execute_NoFilter_IncludesAll()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var u1 = await db.SeedUniverse(u => u.Name = $"All-{Guid.NewGuid()}");
        var u2 = await db.SeedUniverse(u => u.Name = $"All-{Guid.NewGuid()}");

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUniversesUseCase>();

        var input = new GetUniversesInput(new SieveModel { PageSize = 100 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(u => u.Id == u1.Id);
        result.Data.Items.Should().Contain(u => u.Id == u2.Id);
    }

    [Fact]
    public async Task Execute_Pagination_ReturnsTotalCount()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        await db.SeedUniverse(u => u.Name = $"Page-{Guid.NewGuid()}");
        await db.SeedUniverse(u => u.Name = $"Page-{Guid.NewGuid()}");
        await db.SeedUniverse(u => u.Name = $"Page-{Guid.NewGuid()}");

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUniversesUseCase>();

        var input = new GetUniversesInput(new SieveModel { Page = 1, PageSize = 1 });
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().HaveCount(1);
        result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(3);
        result.Data.Page.Should().Be(1);
        result.Data.PageSize.Should().Be(1);
        result.Data.TotalPages.Should().BeGreaterThanOrEqualTo(3);
    }
}
