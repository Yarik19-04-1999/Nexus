using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class DeleteInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_RemovesInviteFromDb()
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        var result = await useCase.Execute(new DeleteInviteInput(invite.Id));

        result.HasError.Should().BeFalse();

        var fromDb = await db.Db.Invites.AsNoTracking().FirstOrDefaultAsync(x => x.Id == invite.Id);
        fromDb.Should().BeNull();
    }

    [Fact]
    public async Task Execute_CascadeDeletesEvents()
    {
        await using var db = new DbScope(_factory);
        var invite = await db.SeedInvite();

        db.Db.InviteEvents.AddRange(
            EventUtils.CreateEvent(invite, InviteEventType.Opened),
            EventUtils.CreateEvent(invite, InviteEventType.SaidYes));
        await db.Db.SaveChangesAsync();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        await useCase.Execute(new DeleteInviteInput(invite.Id));

        var events = await db.Db.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync();
        events.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        var result = await useCase.Execute(new DeleteInviteInput(-999));

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be("NotFound");
    }
}
