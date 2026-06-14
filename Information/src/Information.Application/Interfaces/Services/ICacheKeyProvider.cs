using Information.Application.Enums;

namespace Information.Application.Interfaces.Services;

public interface ICacheKeyProvider
{
    string GetExchangeRatesKey(DateOnly date);
    string GetWeatherHourlyKey(WeatherCity city);
    string GetWeatherDailyKey(WeatherCity city);
}
