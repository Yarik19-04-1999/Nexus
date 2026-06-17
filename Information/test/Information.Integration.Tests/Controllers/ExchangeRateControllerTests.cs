using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Integration.Tests.Extensions;

namespace Information.Integration.Tests.Controllers;

public class ExchangeRateControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly InformationWebApplicationFactory _factory = new();

    [Fact]
    public async Task GetRates_ReturnsOk_AndMapsRatesCorrectly()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var expectedRates = new Dictionary<ExchangeCurrency, ExchangeRate>
        {
            [ExchangeCurrency.USD] = new() { Rate = 41.5m, Date = today },
            [ExchangeCurrency.EUR] = new() { Rate = 44.0m, Date = today },
        };

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.ReplaceWithMock<IGetExchangeRatesUseCase>(mock =>
                mock.Setup(x => x.Execute(It.IsAny<GetExchangeRatesInput>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(expectedRates))))
            .CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/exchangerate");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetExchangeRatesResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.Rates.Should().HaveCount(2);
        body.Rates.Should().Contain(r => r.Currency == ExchangeCurrency.USD && r.Rate.Rate == 41.5m);
        body.Rates.Should().Contain(r => r.Currency == ExchangeCurrency.EUR && r.Rate.Rate == 44.0m);
    }

    [Fact]
    public async Task GetAllHistory_ReturnsOk_AndMapsHistoryCorrectly()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var histories = new List<ExchangeRateHistory>
        {
            new() { Currency = ExchangeCurrency.USD, Current = new() { Rate = 41.5m, Date = today } },
            new() { Currency = ExchangeCurrency.EUR, Current = new() { Rate = 44.0m, Date = today } },
        };

        Mock<IGetExchangeRateHistoryUseCase> mockUseCase = null!;
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
        {
            mockUseCase = s.ReplaceWithMock<IGetExchangeRateHistoryUseCase>(mock =>
                mock.Setup(x => x.Execute(
                        It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(histories));
        })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/exchangerate/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<IReadOnlyList<GetExchangeRateHistoryResponse>>(JsonOptions);
        body.Should().HaveCount(2);
        mockUseCase.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == null),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithCurrency_PassesCurrencyToUseCase()
    {
        // Arrange
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var history = new List<ExchangeRateHistory>
        {
            new() { Currency = ExchangeCurrency.USD, Current = new() { Rate = 41.5m, Date = today } },
        };

        Mock<IGetExchangeRateHistoryUseCase> mockUseCase = null!;
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
        {
            mockUseCase = s.ReplaceWithMock<IGetExchangeRateHistoryUseCase>(mock =>
                mock.Setup(x => x.Execute(
                        It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(history));
        })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/exchangerate/USD/history");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        mockUseCase.Verify(x => x.Execute(
            It.Is<GetExchangeRateHistoryInput>(i => i.Currency == ExchangeCurrency.USD),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHistory_WithInvalidCurrency_ReturnsBadRequest()
    {
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.ReplaceWithMock<IGetExchangeRateHistoryUseCase>()))
            .CreateClient();

        var response = await client.GetAsync("/api/v1/exchangerate/INVALID/history");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
