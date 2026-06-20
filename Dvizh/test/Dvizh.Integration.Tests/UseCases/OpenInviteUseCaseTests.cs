using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Application.Core.Constants;
using Nexus.Core.Integration.Tests.Extensions;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class OpenInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_ReturnsInvite_AndCreatesOpenedEvent()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        var result = await useCase.Execute(new OpenInviteInput(invite.Code), ct);

        result.HasError.Should().BeFalse();
        result.Data.Id.Should().Be(invite.Id);

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id)
            .ToListAsync(ct);
        events.Should().ContainSingle(e => e.EventType == InviteEventType.Opened);
        events[0].Id.Should().BeGreaterThan(0);
        events[0].CreatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_CalledMultipleTimes_CreatesMultipleOpenedEvents()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var invite = await db.SeedInvite();

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        await useCase.Execute(new OpenInviteInput(invite.Code), ct);
        await useCase.Execute(new OpenInviteInput(invite.Code), ct);
        await useCase.Execute(new OpenInviteInput(invite.Code), ct);

        var events = await db.Context.InviteEvents
            .Where(e => e.InviteId == invite.Id && e.EventType == InviteEventType.Opened)
            .ToListAsync(ct);
        events.Should().HaveCount(3);
    }

    [Fact]
    public async Task Execute_WhenNotFound_ReturnsError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IOpenInviteUseCase>();

        var result = await useCase.Execute(new OpenInviteInput("doesntexist"), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
