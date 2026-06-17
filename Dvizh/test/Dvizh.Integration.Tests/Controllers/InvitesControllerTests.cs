using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using Dvizh.Api.Controllers.V1.Invites.CreateInvite;
using Dvizh.Api.Controllers.V1.Invites.GetInviteById;
using Dvizh.Api.Controllers.V1.Invites.GetInvites;
using Dvizh.Api.Controllers.V1.Invites.OpenInvite;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Integration.Tests.Infrastructure;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Integration.Tests.Controllers;

public class InvitesControllerTests
{
    private readonly DvizhWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = CreateFixture();

    private static Fixture CreateFixture()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        return fixture;
    }

    // ── Create ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Create_ReturnsCreated_AndMapsRequestToInput()
    {
        var invite = _fixture.Build<Invite>()
            .With(i => i.Answer, InviteAnswer.Pending)
            .Create();

        var mock = new Mock<ICreateInviteUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<CreateInviteInput>(i =>
                    i.Message == "Hello world" &&
                    i.Language == InviteLanguage.English &&
                    i.Mascot == InviteMascot.UtyaDuck),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Invite>.Success(invite));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var request = new CreateInviteRequest(
            "Hello world", null, null, InviteLanguage.English, InviteMascot.UtyaDuck);

        var response = await client.PostAsJsonAsync("/api/v1/invites", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateInviteResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(invite.Id);
        body.Code.Should().Be(invite.Code);
        body.Message.Should().Be(invite.Message);
        body.Answer.Should().Be(InviteAnswer.Pending);
        mock.Verify(x => x.Execute(
            It.Is<CreateInviteInput>(i =>
                i.Message == "Hello world" &&
                i.Language == InviteLanguage.English &&
                i.Mascot == InviteMascot.UtyaDuck),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Create_WithEmptyMessage_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new CreateInviteRequest("", null, null, InviteLanguage.English, InviteMascot.MochiPeachCat);

        var response = await client.PostAsJsonAsync("/api/v1/invites", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_WithExpiredExpiresAt_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new CreateInviteRequest(
            "Test", null, DateTime.UtcNow.AddDays(-1), InviteLanguage.English, InviteMascot.MochiPeachCat);

        var response = await client.PostAsJsonAsync("/api/v1/invites", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── GetById ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetById_ReturnsOk_AndPassesIdToUseCase()
    {
        var invite = _fixture.Create<Invite>();

        var mock = new Mock<IGetInviteByIdUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetInviteByIdInput>(i => i.Id == 42),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Invite>.Success(invite));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/invites/42");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetInviteByIdResponse>();
        body.Should().NotBeNull();
        body!.Id.Should().Be(invite.Id);
        body.Code.Should().Be(invite.Code);
        body.Message.Should().Be(invite.Message);
        mock.Verify(x => x.Execute(
            It.Is<GetInviteByIdInput>(i => i.Id == 42),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsTeapot()
    {
        var mock = new Mock<IGetInviteByIdUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<GetInviteByIdInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Invite>(999));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/invites/999");

        response.StatusCode.Should().Be((HttpStatusCode)418);
    }

    // ── GetAll ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAll_ReturnsOk_AndPassesExpiryFilterToUseCase()
    {
        var pagedResult = new PagedResult<Invite>([], 0, 1, 20);

        var mock = new Mock<IGetInvitesUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetInvitesInput>(i => i.ExpiryFilter == ExpiryFilter.Active),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<PagedResult<Invite>>.Success(pagedResult));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/invites?expiry=Active");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetInvitesResponse>();
        body.Should().NotBeNull();
        body!.TotalCount.Should().Be(0);
        mock.Verify(x => x.Execute(
            It.Is<GetInvitesInput>(i => i.ExpiryFilter == ExpiryFilter.Active),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Update ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Update_ReturnsOk_AndPassesIdAndFieldsToUseCase()
    {
        var invite = _fixture.Create<Invite>();

        var mock = new Mock<IUpdateInviteUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<UpdateInviteInput>(i =>
                    i.Id == 7 &&
                    i.Message == "Updated" &&
                    i.Language == InviteLanguage.Ukrainian &&
                    i.Mascot == InviteMascot.MochiPeachCat),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Invite>.Success(invite));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var request = new { Id = 7, Message = "Updated", Description = (string?)null, ExpiresAt = (DateTime?)null, Language = InviteLanguage.Ukrainian, Mascot = InviteMascot.MochiPeachCat };
        var response = await client.PutAsJsonAsync("/api/v1/invites", request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mock.Verify(x => x.Execute(
            It.Is<UpdateInviteInput>(i =>
                i.Id == 7 &&
                i.Message == "Updated" &&
                i.Language == InviteLanguage.Ukrainian &&
                i.Mascot == InviteMascot.MochiPeachCat),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsTeapot()
    {
        var mock = new Mock<IUpdateInviteUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<UpdateInviteInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Invite>(999));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var request = new { Id = 999, Message = "X", Description = (string?)null, ExpiresAt = (DateTime?)null, Language = InviteLanguage.Russian, Mascot = InviteMascot.MochiPeachCat };
        var response = await client.PutAsJsonAsync("/api/v1/invites", request);

        response.StatusCode.Should().Be((HttpStatusCode)418);
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Delete_ReturnsNoContent_AndPassesIdToUseCase()
    {
        var mock = new Mock<IDeleteInviteUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<DeleteInviteInput>(i => i.Id == 5),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.DeleteAsync("/api/v1/invites/5");

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        mock.Verify(x => x.Execute(
            It.Is<DeleteInviteInput>(i => i.Id == 5),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsTeapot()
    {
        var mock = new Mock<IDeleteInviteUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<DeleteInviteInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Invite>(999));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.DeleteAsync("/api/v1/invites/999");

        response.StatusCode.Should().Be((HttpStatusCode)418);
    }

    // ── Open ──────────────────────────────────────────────────────────────────

    [Fact]
    public async Task Open_ReturnsOk_AndPassesCodeToUseCase()
    {
        var invite = _fixture.Build<Invite>().With(i => i.Code, "abc123").Create();

        var mock = new Mock<IOpenInviteUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<OpenInviteInput>(i => i.Code == "abc123"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result<Invite>.Success(invite));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/invites/abc123");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<OpenInviteResponse>();
        body.Should().NotBeNull();
        body!.Code.Should().Be("abc123");
        body.Message.Should().Be(invite.Message);
        mock.Verify(x => x.Execute(
            It.Is<OpenInviteInput>(i => i.Code == "abc123"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Open_WhenNotFound_ReturnsTeapot()
    {
        var mock = new Mock<IOpenInviteUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<OpenInviteInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Invite>("nocode"));

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/invites/nocode");

        response.StatusCode.Should().Be((HttpStatusCode)418);
    }

    // ── Respond ───────────────────────────────────────────────────────────────

    [Fact]
    public async Task Respond_ReturnsNoContent_AndPassesCodeAndAnswerToUseCase()
    {
        var mock = new Mock<IRespondToInviteUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<RespondToInviteInput>(i =>
                    i.Code == "abc123" &&
                    i.Answer == InviteAnswer.Yes),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var request = new { Code = "abc123", Answer = InviteAnswer.Yes };
        var response = await client.PostAsJsonAsync("/api/v1/invites/answer", request);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        mock.Verify(x => x.Execute(
            It.Is<RespondToInviteInput>(i =>
                i.Code == "abc123" &&
                i.Answer == InviteAnswer.Yes),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Respond_WithPendingAnswer_ReturnsBadRequest()
    {
        var client = _factory.CreateClient();
        var request = new { Code = "abc123", Answer = InviteAnswer.Pending };

        var response = await client.PostAsJsonAsync("/api/v1/invites/answer", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    // ── ResetAnswer ───────────────────────────────────────────────────────────

    [Fact]
    public async Task ResetAnswer_ReturnsNoContent_AndPassesIdToUseCase()
    {
        var mock = new Mock<IResetInviteAnswerUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<ResetInviteAnswerInput>(i => i.Id == 3),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success());

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddScoped(_ => mock.Object))).CreateClient();

        var response = await client.PostAsync("/api/v1/invites/3/answer/reset", null);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        mock.Verify(x => x.Execute(
            It.Is<ResetInviteAnswerInput>(i => i.Id == 3),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
