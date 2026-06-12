using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Application.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

namespace Information.Application.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _provider;
    private readonly ICacheService _cache;
    private readonly ExchangeRateServiceOptions _options;

    public ExchangeRateService(IExchangeRateProvider provider, ICacheService cache, IOptions<ExchangeRateServiceOptions> options)
    {
        _provider = provider;
        _cache = cache;
        _options = options.Value;
    }

    public Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default)
    {
        var key = $"exchange-rates:{date:yyyyMMdd}";
        var duration = TimeSpan.FromMinutes(_options.CacheDurationMinutes);

        return _cache.GetOrCreate(key, ct => _provider.GetRates(date, ct), duration, cancellationToken);
    }
}
