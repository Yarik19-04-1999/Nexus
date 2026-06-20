using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class ResetInviteAnswerUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_ResetsAnswerToPending_AndCreatesResetEvent()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite(i => i.Answer = InviteAnswer.Yes);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(invite.Id), ct);

        result.HasError.Should().BeFalse();

        db.Context.Entry(invite).State = EntityState.Detached;
        var fromDb = await db.Context.Invites.FindAsync(invite.Id);
        fromDb!.Answer.Should().Be(InviteAnswer.Pending);
        fromDb.UpdatedAt.Should().BeAfter(invite.UpdatedAt);

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync(ct);
        events.Should().ContainSingle(e => e.EventType == InviteEventType.Reset);
        events[0].Id.Should().BeGreaterThan(0);
        events[0].InviteId.Should().Be(invite.Id);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WhenAlreadyPending_StillSucceeds_AndCreatesResetEvent()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(invite.Id), ct);

        result.HasError.Should().BeFalse();

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id && e.EventType == InviteEventType.Reset)
            .ToListAsync(ct);
        events.Should().ContainSingle();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IResetInviteAnswerUseCase>();

        var result = await useCase.Execute(new ResetInviteAnswerInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
