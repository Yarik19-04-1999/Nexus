using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.Nbu;
using Nexus.Application.Core.Exceptions;

namespace Information.Infrastructure.Providers.Nbu;

internal class NbuExchangeRateProvider : IExchangeRateProvider
{
    private static readonly Dictionary<string, ExchangeCurrency> KnownCurrencies =
        ExchangeCurrencies.All.ToDictionary(c => c.ToString(), c => c, StringComparer.OrdinalIgnoreCase);

    private readonly HttpClient _httpClient;

    public NbuExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyDictionary<DateOnly, IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(IReadOnlyList<DateOnly> dates, CancellationToken cancellationToken = default)
    {
        var tasks = dates.Select(date => FetchDate(date, cancellationToken));
        var results = await Task.WhenAll(tasks);
        return dates.Zip(results).ToDictionary(x => x.First, x => x.Second);
    }

    private async Task<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>> FetchDate(DateOnly date, CancellationToken cancellationToken)
    {
        var url = $"/NBUStatService/v1/statdirectory/exchange?date={date:yyyyMMdd}&json";
        var response = await _httpClient.GetFromJsonAsync<List<NbuExchangeRateResponse>>(url, cancellationToken);

        if (response is null)
        {
            throw CommonExceptions.ExternalProviderNoData();
        }

        return response
            .Where(r => KnownCurrencies.ContainsKey(r.Currency))
            .ToDictionary(
                r => KnownCurrencies[r.Currency],
                r => new ExchangeRate { Rate = r.Rate, Date = date });
    }
}
