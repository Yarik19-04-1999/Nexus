using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;
using Xunit;

namespace Dvizh.Integration.Tests.UseCases;

public class CreateInviteUseCaseTests(DvizhWebApplicationFactory factory) : IClassFixture<DvizhWebApplicationFactory>
{
    private readonly DvizhWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_SavesInviteToDb_WithGeneratedCode()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput(TestData.StringValue, null, null, InviteLanguage.Ukrainian, InviteMascot.UtyaDuck);

        var result = await useCase.Execute(input, ct);

        result.HasError.Should().BeFalse();
        result.Data.Id.Should().BeGreaterThan(0);
        result.Data.Code.Should().NotBeNullOrEmpty();
        result.Data.Code.Length.Should().BeLessOrEqualTo(InviteValidationConstants.Invite.CodeMaxLength);
        result.Data.Answer.Should().Be(InviteAnswer.Pending);

        var fromDb = await db.Context.Invites.FindAsync(result.Data.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Code.Should().Be(result.Data.Code);
        fromDb.Message.Should().Be(TestData.StringValue);
        fromDb.Language.Should().Be(InviteLanguage.Ukrainian);
        fromDb.Mascot.Should().Be(InviteMascot.UtyaDuck);
        fromDb.Answer.Should().Be(InviteAnswer.Pending);
        fromDb.CreatedAt.Should().NotBe(default);
        fromDb.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_TwoInvites_HaveDifferentCodes()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput(TestData.StringValue, null, null, InviteLanguage.English, InviteMascot.MochiPeachCat);

        var r1 = await useCase.Execute(input, ct);
        var r2 = await useCase.Execute(input, ct);

        r1.Data.Code.Should().NotBe(r2.Data.Code);
    }

    [Fact]
    public async Task Execute_WithDescription_SavesDescriptionToDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput(TestData.StringValue, TestData.ChangedStringValue, null, InviteLanguage.Russian, InviteMascot.MochiPeachCat);

        var result = await useCase.Execute(input, ct);

        var fromDb = await db.Context.Invites.FindAsync(result.Data.Id);
        fromDb!.Description.Should().Be(TestData.ChangedStringValue);
    }

    [Fact]
    public async Task Execute_WithExpiresAt_SavesExpiresAtToDb()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var expiresAt = DateTime.UtcNow.AddDays(7);
        var input = new CreateInviteInput(TestData.StringValue, null, expiresAt, InviteLanguage.Russian, InviteMascot.MochiPeachCat);

        var result = await useCase.Execute(input, ct);

        var fromDb = await db.Context.Invites.FindAsync(result.Data.Id);
        fromDb!.ExpiresAt.Should().BeCloseTo(expiresAt, TimeSpan.FromSeconds(1));
    }
}
