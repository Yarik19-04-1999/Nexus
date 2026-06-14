using Information.Application.Enums;
using Information.Application.Interfaces.Services;

namespace Information.Application.Services;

public class CacheKeyProvider : ICacheKeyProvider
{
    public string GetExchangeRatesKey(DateOnly date) => $"exchange-rates:{date:yyyyMMdd}";
    public string GetWeatherHourlyKey(WeatherCity city) => $"weather:hourly:{city}";
    public string GetWeatherDailyKey(WeatherCity city) => $"weather:daily:{city}";
}
