using AutoFixture;
using FluentAssertions;
using Information.Api.Controllers.V1.Weather.GetDailyWeather;
using Information.Api.Controllers.V1.Weather.GetHourlyWeather;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Integration.Tests.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nexus.Core.Integration.Tests.Extensions;
using Nexus.Core.Tests.Utils;

namespace Information.Integration.Tests.Controllers;

public class WeatherControllerTests
{
    private readonly InformationWebApplicationFactory _factory = new();
    private readonly Fixture _fixture = FixtureUtils.CreateFixture();

    [Fact]
    public async Task GetHourlyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var hourly = _fixture.Create<List<HourlyWeather>>();

        var mock = new Mock<IGetHourlyWeatherUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(hourly);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/weather/Kharkiv/hourly", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetHourlyWeatherResponse>(ct);
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kharkiv);
        body.Hourly.Should().HaveCount(hourly.Count);
        for (var i = 0; i < hourly.Count; i++)
        {
            body.Hourly[i].Temperature.Should().Be(hourly[i].Temperature);
            body.Hourly[i].WindSpeed.Should().Be(hourly[i].WindSpeed);
            body.Hourly[i].PrecipitationProbability.Should().Be(hourly[i].PrecipitationProbability);
        }
        mock.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kharkiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetDailyWeather_ReturnsOk_AndPassesCityToUseCase()
    {
        var ct = TestContext.Current.CancellationToken;
        var daily = _fixture.Create<List<DailyWeather>>();

        var mock = new Mock<IGetDailyWeatherUseCase>();
        mock.Setup(x => x.Execute(
                It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(daily);

        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/weather/Kyiv/daily", ct);

        response.ShouldBeOk();
        var body = await response.ReadResponse<GetDailyWeatherResponse>(ct);
        body.Should().NotBeNull();
        body!.City.Should().Be(WeatherCity.Kyiv);
        body.Daily.Should().HaveCount(daily.Count);
        for (var i = 0; i < daily.Count; i++)
        {
            body.Daily[i].MaxTemperature.Should().Be(daily[i].MaxTemperature);
            body.Daily[i].MinTemperature.Should().Be(daily[i].MinTemperature);
            body.Daily[i].MaxWindSpeed.Should().Be(daily[i].MaxWindSpeed);
        }
        mock.Verify(x => x.Execute(
            It.Is<GetWeatherInput>(i => i.City == WeatherCity.Kyiv),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetHourlyWeather_WithInvalidCity_ReturnsBadRequest()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IGetHourlyWeatherUseCase>();
        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/weather/Moscow/hourly", ct);

        response.ShouldBeBadRequest();
    }

    [Fact]
    public async Task GetDailyWeather_WithInvalidCity_ReturnsBadRequest()
    {
        var ct = TestContext.Current.CancellationToken;
        var mock = new Mock<IGetDailyWeatherUseCase>();
        var client = _factory.CreateClient(s => s.AddSingleton(mock.Object));

        var response = await client.GetAsync("/api/v1/weather/Moscow/daily", ct);

        response.ShouldBeBadRequest();
    }
}
