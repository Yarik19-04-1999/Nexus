using Dvizh.Application.Constants;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Dvizh.Integration.Tests.UseCases;

public class CreateInviteUseCaseTests : IAsyncDisposable
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly DbScope _db;

    public CreateInviteUseCaseTests()
    {
        _db = new DbScope(_factory);
    }

    [Fact]
    public async Task Execute_SavesInviteToDb_WithGeneratedCode()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput("Приходи завтра", null, null, InviteLanguage.Ukrainian, InviteMascot.UtyaDuck);

        var result = await useCase.Execute(input);

        result.HasError.Should().BeFalse();
        _db.Track(result.Data.Id);

        result.Data.Id.Should().BeGreaterThan(0);
        result.Data.Code.Should().NotBeNullOrEmpty();
        result.Data.Code.Length.Should().BeLessOrEqualTo(InviteValidationConstants.CodeMaxLength);
        result.Data.Answer.Should().Be(InviteAnswer.Pending);

        var fromDb = await _db.Db.Invites.FindAsync(result.Data.Id);
        fromDb.Should().NotBeNull();
        fromDb!.Code.Should().Be(result.Data.Code);
        fromDb.Message.Should().Be("Приходи завтра");
        fromDb.Language.Should().Be(InviteLanguage.Ukrainian);
        fromDb.Mascot.Should().Be(InviteMascot.UtyaDuck);
        fromDb.Answer.Should().Be(InviteAnswer.Pending);
        fromDb.CreatedAt.Should().NotBe(default);
        fromDb.UpdatedAt.Should().NotBe(default);
    }

    [Fact]
    public async Task Execute_TwoInvites_HaveDifferentCodes()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput("Test", null, null, InviteLanguage.English, InviteMascot.MochiPeachCat);

        var r1 = await useCase.Execute(input);
        var r2 = await useCase.Execute(input);

        _db.Track(r1.Data.Id);
        _db.Track(r2.Data.Id);

        r1.Data.Code.Should().NotBe(r2.Data.Code);
    }

    [Fact]
    public async Task Execute_WithDescription_SavesDescriptionToDb()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var input = new CreateInviteInput("Test", "Описание встречи", null, InviteLanguage.Russian, InviteMascot.MochiPeachCat);

        var result = await useCase.Execute(input);
        _db.Track(result.Data.Id);

        var fromDb = await _db.Db.Invites.FindAsync(result.Data.Id);
        fromDb!.Description.Should().Be("Описание встречи");
    }

    [Fact]
    public async Task Execute_WithExpiresAt_SavesExpiresAtToDb()
    {
        using var scope = _factory.Services.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<ICreateInviteUseCase>();

        var expiresAt = DateTime.UtcNow.AddDays(7);
        var input = new CreateInviteInput("Test", null, expiresAt, InviteLanguage.Russian, InviteMascot.MochiPeachCat);

        var result = await useCase.Execute(input);
        _db.Track(result.Data.Id);

        var fromDb = await _db.Db.Invites.FindAsync(result.Data.Id);
        fromDb!.ExpiresAt.Should().BeCloseTo(expiresAt, TimeSpan.FromSeconds(1));
    }

    public async ValueTask DisposeAsync()
    {
        await _db.DisposeAsync();
        _factory.Dispose();
    }
}
