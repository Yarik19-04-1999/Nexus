using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.Weather.GetDailyWeather;
using Information.Api.Controllers.V1.Weather.GetHourlyWeather;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Information.Integration.Tests.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace Information.Integration.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetHourlyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        var hourly = _fixture.Create<List<HourlyWeather>>();

        var mock = new Mock<IGetHourlyWeatherUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(hourly);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/weather/Kharkiv/hourly");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetHourlyWeatherResponse>();
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kharkiv);
        body.Hourly.Should().HaveCount(hourly.Count);
        mock.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDailyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        var daily = _fixture.Create<List<DailyWeather>>();

        var mock = new Mock<IGetDailyWeatherUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(daily);

        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/weather/Kyiv/daily");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<GetDailyWeatherResponse>();
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kyiv);
        body.Daily.Should().HaveCount(daily.Count);
        mock.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHourlyWeather_WithInvalidCity_ReturnsBadRequest()
    {
        var mock = new Mock<IGetHourlyWeatherUseCase>();
        var client = _factory.WithWebHostBuilder(b => b.ConfigureServices(s =>
            s.AddSingleton(mock.Object))).CreateClient();

        var response = await client.GetAsync("/api/v1/weather/Moscow/hourly");

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
