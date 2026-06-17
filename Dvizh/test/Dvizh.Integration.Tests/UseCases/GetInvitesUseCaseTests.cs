using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;

namespace Dvizh.Integration.Tests.UseCases;

public class GetInvitesUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public GetInvitesUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_ExpiryFilter_Active_ExcludesExpiredInvites()
    {
        var active = await _db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await _db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.Active);
        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == active.Id);
        result.Data.Items.Should().NotContain(i => i.Id == expired.Id);
    }

    [Fact]
    public async Task Execute_ExpiryFilter_Expired_ExcludesActiveInvites()
    {
        var active = await _db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await _db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.Expired);
        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == expired.Id);
        result.Data.Items.Should().NotContain(i => i.Id == active.Id);
    }

    [Fact]
    public async Task Execute_ExpiryFilter_All_IncludesBoth()
    {
        var active = await _db.SeedInvite(i =>
        {
            i.Message = $"Active-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(7);
        });
        var expired = await _db.SeedInvite(i =>
        {
            i.Message = $"Expired-{Guid.NewGuid()}";
            i.ExpiresAt = DateTime.UtcNow.AddDays(-1);
        });

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { PageSize = 100 }, ExpiryFilter.All);
        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().Contain(i => i.Id == active.Id);
        result.Data.Items.Should().Contain(i => i.Id == expired.Id);
    }

    [Fact]
    public async Task Execute_Pagination_ReturnsTotalCount()
    {
        await _db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");
        await _db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");
        await _db.SeedInvite(i => i.Message = $"P-{Guid.NewGuid()}");

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetInvitesUseCase>();

        var input = new GetInvitesInput(new SieveModel { Page = 1, PageSize = 1 }, ExpiryFilter.All);
        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        result.Data.Items.Should().HaveCount(1);
        result.Data.TotalCount.Should().BeGreaterThanOrEqualTo(3);
        result.Data.Page.Should().Be(1);
        result.Data.PageSize.Should().Be(1);
        result.Data.TotalPages.Should().BeGreaterThanOrEqualTo(3);
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
