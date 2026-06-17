using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class RespondToInviteUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public RespondToInviteUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Theory]
    [InlineData(InviteAnswer.Yes, InviteEventType.SaidYes)]
    [InlineData(InviteAnswer.No, InviteEventType.SaidNo)]
    public async Task Execute_UpdatesAnswerInDb_AndCreatesCorrectEvent(InviteAnswer answer, InviteEventType expectedEvent)
    {
        var invite = await _db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, answer));

        result.HasError.Should().BeFalse();

        _db.Db.Entry(invite).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        var fromDb = await _db.Db.Invites.FindAsync(invite.Id);
        fromDb!.Answer.Should().Be(answer);
        fromDb.UpdatedAt.Should().BeAfter(invite.UpdatedAt);

        var events = await _db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync();
        events.Should().ContainSingle(e => e.EventType == expectedEvent);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_WhenAlreadyAnswered_ReturnsAlreadyAnsweredError()
    {
        var invite = await _db.SeedInvite(i => i.Answer = InviteAnswer.Yes);

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IRespondToInviteUseCase>();

        var result = await useCase.Execute(new RespondToInviteInput(invite.Code, InviteAnswer.No));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(DvizhErrorCodes.AlreadyAnswered);
    }

    [Fact]
    public async Task Execute_WhenExpired_ReturnsAlreadyExpiredError()
    {
        var invite = await _db.SeedInvite(i => i.ExpiresAt = DateTime.UtcNow.AddDays(-1));

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

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
