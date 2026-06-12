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
    private readonly IExchangeRateProvider _exchangeRateProvider;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyProvider _keysProvider;
    private readonly ExchangeRateCacheOptions _cacheOptions;

    public ExchangeRateService(IExchangeRateProvider exchangeRateProvider, ICacheService cacheService, ICacheKeyProvider keysProvider, IOptions<ExchangeRateCacheOptions> cacheOptions)
    {
        _exchangeRateProvider = exchangeRateProvider;
        _cacheService = cacheService;
        _keysProvider = keysProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default) =>
        await _cacheService.GetOrCreate(_keysProvider.ExchangeRates(date), ct => _exchangeRateProvider.GetRates(date, ct), _cacheOptions.CacheExpiration, cancellationToken);
}
