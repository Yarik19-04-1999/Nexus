using AutoFixture;
using FluentAssertions;
using Lore.Api.Controllers.V1.Universes.CreateUniverse;
using Lore.Api.Controllers.V1.Universes.GetUniverseById;
using Lore.Api.Controllers.V1.Universes.GetUniverses;
using Lore.Api.Controllers.V1.Universes.UpdateUniverse;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;
using Nexus.Core.Tests.Utils;
using System.Linq.Expressions;
using System.Net.Http.Json;

namespace Lore.Integration.Tests.Controllers;

public class UniversesControllerTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetAll_ReturnsOk_AndPassesSieveModelToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var pagedResult = new PagedResult<Universe>([], 0, 1, 20);

        Expression<Func<IGetUniversesUseCase, Task<Result<PagedResult<Universe>>>>> execute = x => x.Execute(
            It.IsAny<GetUniversesInput>(),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IGetUniversesUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<PagedResult<Universe>>.Success(pagedResult));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync("/api/v1/universes", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetUniversesResponse>(ct);
        body.Should().NotBeNull();
        body!.TotalCount.Should().Be(0);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task GetById_ReturnsOk_AndPassesIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var universe = _fixture.Build<Universe>().Without(x => x.Movies).Create();

        Expression<Func<IGetUniverseByIdUseCase, Task<Result<Universe>>>> execute = x => x.Execute(
            It.Is<GetUniverseByIdInput>(i => i.Id == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IGetUniverseByIdUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Universe>.Success(universe));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync($"/api/v1/universes/{TestData.IntValue}", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetUniverseByIdResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(universe.Id);
        body.Name.Should().Be(universe.Name);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task GetById_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IGetUniverseByIdUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<GetUniverseByIdInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Universe>(TestData.NonExistentIntValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.GetAsync($"/api/v1/universes/{TestData.NonExistentIntValue}", ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Create_ReturnsCreated_AndMapsRequestToInput()
    {
        var ct = TestContext.Current.CancellationToken;
        var universe = _fixture.Create<Universe>();

        Expression<Func<ICreateUniverseUseCase, Task<Result<Universe>>>> execute = x => x.Execute(
            It.Is<CreateUniverseInput>(i =>
                i.Name == TestData.StringValue &&
                i.Description == null &&
                i.IsHidden == false &&
                i.ListNo == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<ICreateUniverseUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Universe>.Success(universe));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { Name = TestData.StringValue, Description = (string?)null, IsHidden = false, ListNo = TestData.IntValue };
        var response = await client.PostAsJsonAsync("/api/v1/universes", request, ct);

        response.ShouldBeCreated();
        var body = await response.ReadResponse<CreateUniverseResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(universe.Id);
        body.Name.Should().Be(universe.Name);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Create_WithEmptyName_ReturnsBadRequest()
    {
        var ct = TestContext.Current.CancellationToken;
        var client = _factory.CreateClient();

        var request = new { Name = "", Description = (string?)null, IsHidden = false, ListNo = 0 };
        var response = await client.PostAsJsonAsync("/api/v1/universes", request, ct);

        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task Create_WithNameTooLong_ReturnsBadRequest()
    {
        var ct = TestContext.Current.CancellationToken;
        var client = _factory.CreateClient();

        var request = new { Name = TestData.RandomString(UniverseValidationConstants.NameMaxLength + 1), Description = (string?)null, IsHidden = false, ListNo = 0 };
        var response = await client.PostAsJsonAsync("/api/v1/universes", request, ct);

        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task Update_ReturnsOk_AndPassesFieldsToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var universe = _fixture.Create<Universe>();

        Expression<Func<IUpdateUniverseUseCase, Task<Result<Universe>>>> execute = x => x.Execute(
            It.Is<UpdateUniverseInput>(i =>
                i.Id == TestData.IntValue &&
                i.Name == TestData.ChangedStringValue &&
                i.Description == TestData.StringValue &&
                i.IsHidden == true &&
                i.ListNo == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IUpdateUniverseUseCase>();
        mock.Setup(execute).ReturnsAsync(Result<Universe>.Success(universe));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { Id = TestData.IntValue, Name = TestData.ChangedStringValue, Description = TestData.StringValue, IsHidden = true, ListNo = TestData.IntValue };
        var response = await client.PutAsJsonAsync("/api/v1/universes", request, ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<UpdateUniverseResponse>(ct);
        body.Should().NotBeNull();
        body!.Id.Should().Be(universe.Id);
        body.Name.Should().Be(universe.Name);
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Update_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IUpdateUniverseUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<UpdateUniverseInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Universe>(int.MaxValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var request = new { Id = int.MaxValue, Name = TestData.StringValue, Description = (string?)null, IsHidden = false, ListNo = 0 };
        var response = await client.PutAsJsonAsync("/api/v1/universes", request, ct);

        response.ShouldBeDomainError();
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_AndPassesIdToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;

        Expression<Func<IDeleteUniverseUseCase, Task<Result>>> execute = x => x.Execute(
            It.Is<DeleteUniverseInput>(i => i.Id == TestData.IntValue),
            It.IsAny<CancellationToken>());

        var mock = new Mock<IDeleteUniverseUseCase>();
        mock.Setup(execute).ReturnsAsync(Result.Success());

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.DeleteAsync($"/api/v1/universes/{TestData.IntValue}", ct);

        response.ShouldBeNoContent();
        mock.Verify(execute, Times.Once);
    }

    [Fact]
    public async Task Delete_WhenNotFound_ReturnsDomainError()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IDeleteUniverseUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<DeleteUniverseInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ResultConstants.NotFound<Universe>(TestData.NonExistentIntValue));

        var client = _factory.CreateClient(s => s.AddScoped(_ => mock.Object));

        var response = await client.DeleteAsync($"/api/v1/universes/{TestData.NonExistentIntValue}", ct);

        response.ShouldBeDomainError();
    }
}
