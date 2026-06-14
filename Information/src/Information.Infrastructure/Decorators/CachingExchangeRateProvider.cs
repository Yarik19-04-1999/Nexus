using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Decorators;

internal class CachingExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateProvider _inner;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyProvider _cacheKeyProvider;
    private readonly ExchangeRateCacheOptions _cacheOptions;

    public CachingExchangeRateProvider(IExchangeRateProvider inner, ICacheService cacheService, ICacheKeyProvider cacheKeyProvider, IOptions<ExchangeRateCacheOptions> cacheOptions)
    {
        _inner = inner;
        _cacheService = cacheService;
        _cacheKeyProvider = cacheKeyProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default) =>
        _cacheService.GetOrCreate(_cacheKeyProvider.GetExchangeRatesKey(date), ct => _inner.GetRates(date, ct), _cacheOptions.CacheExpiration, cancellationToken);
}
