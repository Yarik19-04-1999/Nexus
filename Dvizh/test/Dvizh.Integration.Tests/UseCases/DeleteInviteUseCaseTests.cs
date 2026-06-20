using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class DeleteInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_RemovesInviteFromDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        var result = await useCase.Execute(new DeleteInviteInput(invite.Id), ct);

        result.HasError.Should().BeFalse();

        var fromDb = await db.Context.Invites.AsNoTracking().FirstOrDefaultAsync(x => x.Id == invite.Id, ct);
        fromDb.Should().BeNull();
    }

    [Fact]
    public async Task Execute_CascadeDeletesEvents()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        db.Context.InviteEvents.AddRange(
            EventUtils.CreateEvent(invite, InviteEventType.Opened),
            EventUtils.CreateEvent(invite, InviteEventType.SaidYes));
        await db.Context.SaveChangesAsync(ct);

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        await useCase.Execute(new DeleteInviteInput(invite.Id), ct);

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync(ct);
        events.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IDeleteInviteUseCase>();

        var result = await useCase.Execute(new DeleteInviteInput(-999), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
