using Information.Application.Interfaces.Services;

namespace Information.Application.Services;

public class CacheKeyProvider : ICacheKeyProvider
{
    public string ExchangeRates(DateOnly date) => $"exchange-rates:{date:yyyyMMdd}";
}
