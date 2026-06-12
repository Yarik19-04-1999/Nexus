using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

namespace Information.Application.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly IExchangeRateProvider _provider;
    private readonly ICacheService _cache;
    private readonly ICacheKeyProvider _keys;
    private readonly ExchangeRateCacheOptions _options;

    public ExchangeRateService(IExchangeRateProvider provider, ICacheService cache, ICacheKeyProvider keys, IOptions<ExchangeRateCacheOptions> options)
    {
        _provider = provider;
        _cache = cache;
        _keys = keys;
        _options = options.Value;
    }

    public Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default) =>
        _cache.GetOrCreate(_keys.ExchangeRates(date), ct => _provider.GetRates(date, ct), _options.CacheExpiration, cancellationToken);
}
