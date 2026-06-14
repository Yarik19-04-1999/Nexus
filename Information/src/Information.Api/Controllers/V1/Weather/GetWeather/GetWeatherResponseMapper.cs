using Information.Api.Controllers.V1.Weather.GetWeather.Dtos;
using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.Weather.GetWeather;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetWeatherResponseMapper
{
    public static partial GetWeatherResponse Map(CityWeather city);
}
