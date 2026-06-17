using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Integration.Tests.Utils;

namespace Information.Integration.Tests.Controllers;

public class ExchangeRateControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetRates_ReturnsOk_AndMapsRatesCorrectly(CancellationToken cancellationToken)
    {
        var rates = _fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>();

        var mock = new Mock<IGetExchangeRatesUseCase>();
        mock.Setup(x => x.Execute(GetExchangeRatesInput.Instance, It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/exchangerate", cancellationToken);

        response.ShouldBeOk();
        var body = await response.ReadJsonResponse<GetExchangeRatesResponse>(cancellationToken);
        body.Should().NotBeNull();
        body!.Rates.Should().HaveCount(rates.Count);
        var (expectedCurrency, expectedRate) = rates.First();
        body.Rates.Should().Contain(r =>
            r.Currency == expectedCurrency &&
            r.Rate.Rate == expectedRate.Rate &&
            r.Rate.Date == expectedRate.Date);
        mock.Verify(x => x.Execute(GetExchangeRatesInput.Instance, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetAllHistory_ReturnsOk_PassesNullCurrencyToUseCase(CancellationToken cancellationToken)
    {
        var histories = _fixture.Create<List<ExchangeRateHistory>>();

        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(histories);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/exchangerate/history", cancellationToken);

        response.ShouldBeOk();
        var body = await response.ReadJsonResponse<IReadOnlyList<GetExchangeRateHistoryResponse>>(cancellationToken);
        body.Should().HaveCount(histories.Count);
        mock.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithCurrency_PassesCurrencyToUseCase(CancellationToken cancellationToken)
    {
        var history = _fixture.Create<List<ExchangeRateHistory>>();

        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(history);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/exchangerate/USD/history", cancellationToken);

        response.ShouldBeOk();
        mock.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithInvalidCurrency_ReturnsBadRequest(CancellationToken cancellationToken)
    {
        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/exchangerate/INVALID/history", cancellationToken);

        response.ShouldBeBadRequest();
    }
}
