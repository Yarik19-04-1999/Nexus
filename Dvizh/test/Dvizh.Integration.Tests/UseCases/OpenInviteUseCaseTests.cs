using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class OpenInviteUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public OpenInviteUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_ReturnsInvite_AndCreatesOpenedEvent()
    {
        var invite = await _db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        var result = await useCase.Execute(new OpenInviteInput(invite.Code));

        result.HasError.Should().BeFalse();
        result.Data.Id.Should().Be(invite.Id);
        result.Data.Code.Should().Be(invite.Code);

        var events = await _db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync();
        events.Should().ContainSingle(e => e.EventType == InviteEventType.Opened);
        events[0].Id.Should().BeGreaterThan(0);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_CalledMultipleTimes_CreatesMultipleOpenedEvents()
    {
        var invite = await _db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        await useCase.Execute(new OpenInviteInput(invite.Code));
        await useCase.Execute(new OpenInviteInput(invite.Code));
        await useCase.Execute(new OpenInviteInput(invite.Code));

        var events = await _db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id && e.EventType == InviteEventType.Opened)
            .ToListAsync();
        events.Should().HaveCount(3);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        var result = await useCase.Execute(new OpenInviteInput("doesntexist"));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
