using Information.Api.Controllers.V1.Weather.GetWeather.Dtos;
using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.Weather.GetDailyWeather;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetDailyWeatherResponseMapper
{
    public static partial DailyWeatherDto Map(DailyWeather daily);
}
