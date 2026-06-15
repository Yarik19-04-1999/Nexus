using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;

namespace Information.Application.UseCases;

public class GetDailyWeatherUseCase : IGetDailyWeatherUseCase
{
    private readonly IWeatherProvider _weatherProvider;

    public GetDailyWeatherUseCase(IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    public async Task<IReadOnlyList<DailyWeather>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
        => await _weatherProvider.GetDailyForecast(input.City, cancellationToken);
}
