using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.Nbu;
using Information.Infrastructure.Options;
using Information.Infrastructure.Services;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Providers.Nbu;

internal class NbuExchangeRateProvider : IExchangeRateProvider
{
    private const string BaseUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange";
    private const string SourceName = "NBU";

    private readonly HttpClient _http;
    private readonly ICacheService _cache;
    private readonly ExchangeRateOptions _options;

    public NbuExchangeRateProvider(HttpClient http, ICacheService cache, IOptions<ExchangeRateOptions> options)
    {
        _http = http;
        _cache = cache;
        _options = options.Value;
    }

    public Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default)
    {
        var key = $"nbu:{date:yyyyMMdd}";
        var duration = TimeSpan.FromMinutes(_options.CacheDurationMinutes);

        return _cache.GetOrCreate(key, ct => FetchRates(date, ct), duration, cancellationToken);
    }

    private async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> FetchRates(DateOnly date, CancellationToken cancellationToken)
    {
        try
        {
            var url = $"{BaseUrl}?date={date:yyyyMMdd}&json";
            var response = await _http.GetFromJsonAsync<List<NbuExchangeRateResponse>>(url, cancellationToken);

            if (response is null)
            {
                return InformationResultConstants.ProviderUnavailable<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>(SourceName);
            }

            var knownCurrencies = Enum.GetValues<ExchangeCurrency>()
                .ToDictionary(c => c.ToString(), c => c, StringComparer.OrdinalIgnoreCase);

            var rates = response
                .Where(r => knownCurrencies.ContainsKey(r.Currency))
                .ToDictionary(
                    r => knownCurrencies[r.Currency],
                    r => new ExchangeRate { Rate = r.Rate, Date = date });

            return Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>.Success(rates);
        }
        catch (Exception)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>(SourceName, canRetry: true);
        }
    }
}
