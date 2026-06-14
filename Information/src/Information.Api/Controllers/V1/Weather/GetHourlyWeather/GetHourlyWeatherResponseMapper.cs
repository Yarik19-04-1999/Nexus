using Information.Api.Controllers.V1.Weather.GetWeather.Dtos;
using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.Weather.GetHourlyWeather;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetHourlyWeatherResponseMapper
{
    public static partial HourlyWeatherDto Map(HourlyWeather hourly);
}
