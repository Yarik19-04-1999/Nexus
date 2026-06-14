using Information.Api.Controllers.V1.Weather.GetDailyWeather;
using Information.Api.Controllers.V1.Weather.GetHourlyWeather;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;

namespace Information.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class WeatherController : ControllerBase
{
    [HttpGet("{city}/hourly")]
    public async Task<IActionResult> GetHourlyWeather(
        WeatherCity city,
        [FromServices] IGetHourlyWeatherUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetWeatherInput(city), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(new GetHourlyWeatherResponse(city, result.Data.Select(GetHourlyWeatherResponseMapper.Map).ToList()));
    }

    [HttpGet("{city}/daily")]
    public async Task<IActionResult> GetDailyWeather(
        WeatherCity city,
        [FromServices] IGetDailyWeatherUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetWeatherInput(city), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(new GetDailyWeatherResponse(city, result.Data.Select(GetDailyWeatherResponseMapper.Map).ToList()));
    }
}
