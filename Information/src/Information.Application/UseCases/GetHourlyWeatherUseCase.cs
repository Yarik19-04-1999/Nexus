using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetHourlyWeatherUseCase : IGetHourlyWeatherUseCase
{
    private readonly IWeatherProvider _weatherProvider;

    public GetHourlyWeatherUseCase(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    public async Task<Result<IReadOnlyList<HourlyWeather>>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
    {
        var result = await _weatherProvider.GetHourlyForecast(input.City, cancellationToken);

        if (result.HasError)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<HourlyWeather>>(nameof(IWeatherProvider));
        }

        return result;
    }
}
