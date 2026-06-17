using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class DeleteInviteUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public DeleteInviteUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_RemovesInviteFromDb()
    {
        var invite = await _db.SeedInvite();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        var result = await useCase.Execute(new DeleteInviteInput(invite.Id));

        result.HasError.Should().BeFalse();

        var fromDb = await _db.Db.Invites.FindAsync(invite.Id);
        fromDb.Should().BeNull();
    }

    [Fact]
    public async Task Execute_CascadeDeletesEvents()
    {
        var invite = await _db.SeedInvite();

        _db.Db.InviteEvents.AddRange(
            EventUtils.CreateEvent(invite, InviteEventType.Opened),
            EventUtils.CreateEvent(invite, InviteEventType.SaidYes));
        await _db.Db.SaveChangesAsync();

        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        await useCase.Execute(new DeleteInviteInput(invite.Id));

        var events = await _db.Db.InviteEvents
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

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
