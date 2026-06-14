using Information.Application.Constants;
using Information.Application.Interfaces.Services;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetDailyWeatherUseCase : IGetDailyWeatherUseCase
{
    private readonly IWeatherService _weatherService;

    public GetDailyWeatherUseCase(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<Result<IReadOnlyList<DailyWeather>>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
    {
        var result = await _weatherService.GetWeather(input.City, cancellationToken);

        if (result.HasError)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<DailyWeather>>(nameof(IWeatherService));
        }

        return Result<IReadOnlyList<DailyWeather>>.Success(result.Data.Daily);
    }
}
