using Information.Application.Enums;

namespace Information.Application.Interfaces.Services;

public interface ICacheKeyProvider
{
    string GetExchangeRatesKey(DateOnly date);
    string GetWeatherKey(WeatherCity city);
}
