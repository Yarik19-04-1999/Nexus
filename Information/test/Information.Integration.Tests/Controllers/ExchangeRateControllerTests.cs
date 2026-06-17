using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Information.Integration.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Information.Integration.Tests.Controllers;

public class ExchangeRateControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetRates_ReturnsOk_AndMapsRatesCorrectly()
    {
        var rates = _fixture.Create<Dictionary<ExchangeCurrency, ExchangeRate>>();

        var mock = new Mock<IGetExchangeRatesUseCase>();
        mock.Setup(x => x.Execute(It.IsAny<GetExchangeRatesInput>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(rates);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/exchangerate");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetExchangeRatesResponse>();
        body.Should().NotBeNull();
        body!.Rates.Should().HaveCount(rates.Count);
    }

    [Fact]
    public async Task GetAllHistory_ReturnsOk_PassesNullCurrencyToUseCase()
    {
        var histories = _fixture.Create<List<ExchangeRateHistory>>();

        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(histories);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/exchangerate/history");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IReadOnlyList<GetExchangeRateHistoryResponse>>();
        body.Should().HaveCount(histories.Count);
        mock.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithCurrency_PassesCurrencyToUseCase()
    {
        var history = _fixture.Create<List<ExchangeRateHistory>>();

        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(history);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/exchangerate/USD/history");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mock.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithInvalidCurrency_ReturnsBadRequest()
    {
        var mock = new Mock<IGetExchangeRateHistoryUseCase>();
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/exchangerate/INVALID/history");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
