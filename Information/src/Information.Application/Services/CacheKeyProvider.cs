using Information.Application.Enums;
using Information.Application.Interfaces.Services;

namespace Information.Application.Services;

public class CacheKeyProvider : ICacheKeyProvider
{
    public string GetExchangeRatesKey(DateOnly date) => $"exchange-rates:{date:yyyyMMdd}";
    public string GetWeatherKey(WeatherCity city) => $"weather:{city}";
}
