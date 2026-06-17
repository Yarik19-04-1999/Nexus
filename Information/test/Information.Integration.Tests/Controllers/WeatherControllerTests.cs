using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Information.Api.Controllers.V1.Weather.GetDailyWeather;
using Information.Api.Controllers.V1.Weather.GetHourlyWeather;
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

public class WeatherControllerTests
{
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };
    private readonly InformationWebApplicationFactory _factory = new();

    [Fact]
    public async Task GetHourlyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        // Arrange
        var hourly = new List<HourlyWeather>
        {
            new()
            {
                Time = DateTime.UtcNow,
                Temperature = 22.5,
                ApparentTemperature = 21.0,
                PrecipitationProbability = 10,
                WeatherCode = WeatherCode.ClearSky,
                WindSpeed = 5.0,
            }
        };

        Mock<IGetHourlyWeatherUseCase> mockUseCase = null!;
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
        {
            mockUseCase = s.ReplaceWithMock<IGetHourlyWeatherUseCase>(mock =>
                mock.Setup(x => x.Execute(
                        It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(hourly));
        })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/weather/Kharkiv/hourly");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetHourlyWeatherResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kharkiv);
        body.Hourly.Should().HaveCount(1);
        body.Hourly[0].Temperature.Should().Be(22.5);
        mockUseCase.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDailyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        // Arrange
        var daily = new List<DailyWeather>
        {
            new()
            {
                Date = DateOnly.FromDateTime(DateTime.Today),
                MaxTemperature = 28.0,
                MinTemperature = 17.0,
                PrecipitationSum = 0.0,
                PrecipitationProbability = 5,
                WeatherCode = WeatherCode.ClearSky,
                MaxWindSpeed = 10.0,
            }
        };

        Mock<IGetDailyWeatherUseCase> mockUseCase = null!;
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
        {
            mockUseCase = s.ReplaceWithMock<IGetDailyWeatherUseCase>(mock =>
                mock.Setup(x => x.Execute(
                        It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(daily));
        })).CreateClient();

        // Act
        var response = await client.GetAsync("/api/v1/weather/Kyiv/daily");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetDailyWeatherResponse>(JsonOptions);
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kyiv);
        body.Daily.Should().HaveCount(1);
        body.Daily[0].MaxTemperature.Should().Be(28.0);
        mockUseCase.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHourlyWeather_WithInvalidCity_ReturnsBadRequest()
    {
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.ReplaceWithMock<IGetHourlyWeatherUseCase>()))
            .CreateClient();

        var response = await client.GetAsync("/api/v1/weather/Moscow/hourly");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
