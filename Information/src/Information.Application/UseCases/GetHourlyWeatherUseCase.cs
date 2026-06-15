using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;

namespace Information.Application.UseCases;

public class GetHourlyWeatherUseCase : IGetHourlyWeatherUseCase
{
    private readonly IWeatherProvider _weatherProvider;

    public GetHourlyWeatherUseCase(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    public async Task<IReadOnlyList<HourlyWeather>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
        => await _weatherProvider.GetHourlyForecast(input.City, cancellationToken);
}
