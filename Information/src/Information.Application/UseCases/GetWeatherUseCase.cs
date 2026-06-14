using Information.Application.Constants;
using Information.Application.Interfaces.Services;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetWeatherUseCase : IGetWeatherUseCase
{
    private readonly IWeatherService _weatherService;

    public GetWeatherUseCase(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<Result<CityWeather>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
    {
        var result = await _weatherService.GetWeather(input.City, cancellationToken);

        if (result.HasError)
        {
            return InformationResultConstants.ProviderUnavailable<CityWeather>(nameof(IWeatherService));
        }

        return result;
    }
}
