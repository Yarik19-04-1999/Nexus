using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class UpdateInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_UpdatesFieldsInDb(CancellationToken cancellationToken)
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite(i =>
        {
            i.Message = "Старое сообщение";
            i.Language = InviteLanguage.Russian;
            i.Mascot = InviteMascot.MochiPeachCat;
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        var input = new UpdateInviteInput(invite.Id, "Новое сообщение", "Описание", null, InviteLanguage.Ukrainian, InviteMascot.UtyaDuck);
        var result = await useCase.Execute(input, cancellationToken);

        result.HasError.Should().BeFalse();
        result.Data.Message.Should().Be("Новое сообщение");
        result.Data.Language.Should().Be(InviteLanguage.Ukrainian);
        result.Data.Mascot.Should().Be(InviteMascot.UtyaDuck);

        db.Db.Entry(invite).State = EntityState.Detached;
        var fromDb = await db.Db.Invites.FindAsync(invite.Id);
        fromDb!.Message.Should().Be("Новое сообщение");
        fromDb.Description.Should().Be("Описание");
        fromDb.Language.Should().Be(InviteLanguage.Ukrainian);
        fromDb.Mascot.Should().Be(InviteMascot.UtyaDuck);
    }

    [Fact]
    public async Task Execute_UpdatesUpdatedAt(CancellationToken cancellationToken)
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite();
        var originalUpdatedAt = invite.UpdatedAt;

        await Task.Delay(10, cancellationToken);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        await useCase.Execute(new UpdateInviteInput(invite.Id, "Changed", null, null, InviteLanguage.Russian, InviteMascot.MochiPeachCat), cancellationToken);

        db.Db.Entry(invite).State = EntityState.Detached;
        var fromDb = await db.Db.Invites.FindAsync(invite.Id);
        fromDb!.UpdatedAt.Should().BeAfter(originalUpdatedAt);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError(CancellationToken cancellationToken)
    {
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IUpdateInviteUseCase>();

        var result = await useCase.Execute(new UpdateInviteInput(-999, "X", null, null, InviteLanguage.Russian, InviteMascot.MochiPeachCat), cancellationToken);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }
}
