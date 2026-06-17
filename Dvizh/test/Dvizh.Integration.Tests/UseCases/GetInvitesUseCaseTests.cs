using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Sieve.Models;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class GetInvitesUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_ExpiryFilter_Active_ExcludesExpiredInvites()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DbScope(_factory);
        var active = await db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.Active);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == active.Id);
        result.Data.Items.Should().NotContain(i => i.Id == expired.Id);
    }

    [Fact]
    public async Task Execute_ExpiryFilter_Expired_ExcludesActiveInvites()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DbScope(_factory);
        var active = await db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.Expired);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == expired.Id);
        result.Data.Items.Should().NotContain(i => i.Id == active.Id);
    }

    [Fact]
    public async Task Execute_ExpiryFilter_All_IncludesBoth()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DbScope(_factory);
        var active = await db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.All);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == active.Id);
        result.Data.Items.Should().Contain(i => i.Id == expired.Id);
    }

    [Fact]
    public async Task Execute_Pagination_ReturnsTotalCount()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DbScope(_factory);
        await db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");
        await db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");
        await db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { Page = 1, PageSize = 1 }, ExpiryFilter.All);
        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().HaveCount(1);
        result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(3);
        result.Data.Page.Should().Be(1);
        result.Data.PageSize.Should().Be(1);
        result.Data.TotalPages.Should().BeGreaterThanOrEqualTo(3);
    }
}
