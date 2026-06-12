using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetWeatherUseCase : IGetWeatherUseCase
{
    private readonly IWeatherProvider _provider;

    public GetWeatherUseCase(IWeatherProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<WeatherInfo>> Execute(GetWeatherInput input, CancellationToken cancellationToken = default)
    {
        return await _provider.GetWeather(input.City, cancellationToken);
    }
}
