using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, answer));

        result.HasError.Should().BeFalse();

        db.Db.Entry(invite).State = EntityState.Detached;
        var fromDb = await db.Db.Invites.FindAsync(invite.Id);
        fromDb!.Answer.Should().Be(answer);
        fromDb.UpdatedAt.Should().BeAfter(invite.UpdatedAt);

        var events = await db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync();
        events.Should().ContainSingle(e => e.EventType == expectedEvent);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WhenAlreadyAnswered_ReturnsAlreadyAnsweredError()
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite(i => i.Answer = InviteAnswer.Yes);

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, InviteAnswer.No));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(DvizhErrorCodes.AlreadyAnswered);
    }

    [Fact]
    public async Task Execute_WhenExpired_ReturnsAlreadyExpiredError()
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite(i => i.ExpiresAt = DateTime.UtcNow.AddDays(-1));

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, InviteAnswer.Yes));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("AlreadyExpired");
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsNotFoundError()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput("doesntexist", InviteAnswer.Yes));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }
}
