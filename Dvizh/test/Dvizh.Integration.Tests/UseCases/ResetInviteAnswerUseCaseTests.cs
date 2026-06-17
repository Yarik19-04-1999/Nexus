using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class ResetInviteAnswerUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public ResetInviteAnswerUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_ResetsAnswerToPending_AndCreatesResetEvent()
    {
        var invite = await _db.SeedInvite(i => i.Answer = InviteAnswer.Yes);

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(invite.Id));

        result.HasError.Should().BeFalse();

        _db.Db.Entry(invite).State = EntityState.Detached;
        var fromDb = await _db.Db.Invites.FindAsync(invite.Id);
        fromDb!.Answer.Should().Be(InviteAnswer.Pending);
        fromDb.UpdatedAt.Should().BeAfter(invite.UpdatedAt);

        var events = await _db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync();
        events.Should().ContainSingle(e => e.EventType == InviteEventType.Reset);
        events[0].Id.Should().BeGreaterThan(0);
        events[0].InviteId.Should().Be(invite.Id);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WhenAlreadyPending_StillSucceeds_AndCreatesResetEvent()
    {
        var invite = await _db.SeedInvite(); // Answer = Pending by default

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(invite.Id));

        result.HasError.Should().BeFalse();

        var events = await _db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id && e.EventType == InviteEventType.Reset)
            .ToListAsync();
        events.Should().ContainSingle();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(-999));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
