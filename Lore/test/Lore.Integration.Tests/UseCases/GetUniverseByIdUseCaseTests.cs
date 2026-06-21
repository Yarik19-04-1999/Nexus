using FluentAssertions;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class GetUniverseByIdUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_ReturnsUniverse()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var universe = await db.SeedUniverse(u =>
        {
            u.Name = TestData.StringValue;
            u.Description = TestData.ChangedStringValue;
            u.IsHidden = true;
            u.ListNo = TestData.IntValue;
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUniverseByIdUseCase>();

        var result = await useCase.Execute(new GetUniverseByIdInput(universe.Id), ct);

        result.HasError.Should().BeFalse();
        result.Data.Id.Should().Be(universe.Id);
        result.Data.Name.Should().Be(TestData.StringValue);
        result.Data.Description.Should().Be(TestData.ChangedStringValue);
        result.Data.IsHidden.Should().BeTrue();
        result.Data.ListNo.Should().Be(TestData.IntValue);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetUniverseByIdUseCase>();

        var result = await useCase.Execute(new GetUniverseByIdInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
