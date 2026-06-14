using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetDailyWeatherUseCase : IGetDailyWeatherUseCase
{
    private readonly IWeatherProvider _weatherProvider;

    public GetDailyWeatherUseCase(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    public async Task<Result<IReadOnlyList<DailyWeather>>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
    {
        var result = await _weatherProvider.GetDailyForecast(input.City, cancellationToken);

        if (result.HasError)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<DailyWeather>>(nameof(IWeatherProvider));
        }

        return result;
    }
}
