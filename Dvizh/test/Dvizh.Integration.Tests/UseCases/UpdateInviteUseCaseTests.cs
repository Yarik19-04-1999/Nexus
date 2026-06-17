using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class UpdateInviteUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public UpdateInviteUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_UpdatesFieldsInDb()
    {
        var invite = await _db.SeedInvite(i =>
        {
            i.Message = "Старое сообщение";
            i.Language = InviteLanguage.Russian;
            i.Mascot = InviteMascot.MochiPeachCat;
        });

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        var input = new UpdateInviteInput(invite.Id, "Новое сообщение", "Описание", null, InviteLanguage.Ukrainian, InviteMascot.UtyaDuck);

        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        result.Data.Message.Should().Be("Новое сообщение");
        result.Data.Description.Should().Be("Описание");
        result.Data.Language.Should().Be(InviteLanguage.Ukrainian);
        result.Data.Mascot.Should().Be(InviteMascot.UtyaDuck);

        _db.Db.Entry(invite).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        var fromDb = await _db.Db.Invites.FindAsync(invite.Id);
        fromDb!.Message.Should().Be("Новое сообщение");
        fromDb.Description.Should().Be("Описание");
        fromDb.Language.Should().Be(InviteLanguage.Ukrainian);
        fromDb.Mascot.Should().Be(InviteMascot.UtyaDuck);
    }

    [Fact]
    public async Task Execute_UpdatesUpdatedAt()
    {
        var invite = await _db.SeedInvite();
        var originalUpdatedAt = invite.UpdatedAt;

        await Task.Delay(10); // ensure time advances

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        var input = new UpdateInviteInput(invite.Id, "Changed", null, null, InviteLanguage.Russian, InviteMascot.MochiPeachCat);
        await useCase.Execute(input);

        _db.Db.Entry(invite).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        var fromDb = await _db.Db.Invites.FindAsync(invite.Id);
        fromDb!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        var input = new UpdateInviteInput(-999, "X", null, null, InviteLanguage.Russian, InviteMascot.MochiPeachCat);

        var result = await useCase.Execute(input);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
