using Dvizh.Application.Constants;
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

public class RespondToInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Theory]
    [InlineData(InviteAnswer.Yes, InviteEventType.SaidYes)]
    [InlineData(InviteAnswer.No, InviteEventType.SaidNo)]
    public async Task Execute_UpdatesAnswerInDb_AndCreatesCorrectEvent(InviteAnswer answer, InviteEventType expectedEvent)
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, answer), ct);

        result.HasError.Should().BeFalse();

        db.Context.Entry(invite).State = EntityState.Detached;
        var fromDb = await db.Context.Invites.FindAsync(invite.Id);
        fromDb!.Answer.Should().Be(answer);
        fromDb.UpdatedAt.Should().BeAfter(invite.UpdatedAt);

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync(ct);
        events.Should().ContainSingle(e => e.EventType == expectedEvent);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WhenAlreadyAnswered_ReturnsAlreadyAnsweredError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite(i => i.Answer = InviteAnswer.Yes);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, InviteAnswer.No), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(DvizhErrorCodes.AlreadyAnswered);
    }

    [Fact]
    public async Task Execute_WhenExpired_ReturnsAlreadyExpiredError()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite(i => i.ExpiresAt = DateTime.UtcNow.AddDays(-1));

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, InviteAnswer.Yes), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.AlreadyExpired);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(TestData.RandomString(InviteValidationConstants.Invite.CodeMaxLength), InviteAnswer.Yes), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
