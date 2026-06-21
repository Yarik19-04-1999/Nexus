using Nexus.Application.Core.Constants;
using FluentAssertions;
using Lore.Application.Constants;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Lore.Application.Models.Inputs;
using Lore.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Constants;

namespace Lore.Integration.Tests.UseCases;

public class GetMovieByIdUseCaseTests(LoreWebApplicationFactory factory) : IClassFixture<LoreWebApplicationFactory>
{
    private readonly LoreWebApplicationFactory _factory = factory;

    [Fact]
    public async Task Execute_WhenMovieExists_ReturnsAllFields()
    {
        var ct = TestContext.Current.CancellationToken;
        await using var db = new DatabaseScope(_factory);
        var movie = await db.SeedMovie(m =>
        {
            m.Title = TestData.StringValue;
            m.ReleaseYear = 2023;
            m.DurationMinutes = 150;
            m.ReviewText = TestData.ChangedStringValue;
            m.Score = 8.5m;
            m.ViewCount = 2;
            m.RewatchStatus = RewatchStatus.OptionalRewatch;
            m.ListNo = TestData.IntValue;
        });

        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetMovieByIdUseCase>();

        var result = await useCase.Execute(new GetMovieByIdInput(movie.Id), ct);

        result.HasError.Should().BeFalse();
        result.Data!.Id.Should().Be(movie.Id);
        result.Data.Title.Should().Be(TestData.StringValue);
        result.Data.ReleaseYear.Should().Be(2023);
        result.Data.DurationMinutes.Should().Be(150);
        result.Data.ReviewText.Should().Be(TestData.ChangedStringValue);
        result.Data.Score.Should().Be(8.5m);
        result.Data.ViewCount.Should().Be(2);
        result.Data.RewatchStatus.Should().Be(RewatchStatus.OptionalRewatch);
        result.Data.ListNo.Should().Be(TestData.IntValue);
    }

    [Fact]
    public async Task Execute_WhenMovieNotFound_ReturnsMovieNotFoundError()
    {
        var ct = TestContext.Current.CancellationToken;
        using var scope = _factory.CreateScope();
        var useCase = scope.ServiceProvider.GetRequiredService<IGetMovieByIdUseCase>();

        var result = await useCase.Execute(new GetMovieByIdInput(TestData.NonExistentIntValue), ct);

        result.HasError.Should().BeTrue();
        result.ErrorCode.Should().Be(CommonErrorCodes.NotFound);
    }
}
