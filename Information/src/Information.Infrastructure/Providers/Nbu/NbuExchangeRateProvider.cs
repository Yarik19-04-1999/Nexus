using System.Net.Http.Json;
using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Models;
using Information.Infrastructure.Models.Nbu;
using Nexus.Application.Core.Models;

namespace Information.Infrastructure.Providers.Nbu;

internal class NbuExchangeRateProvider : IExchangeRateProvider
{
    private const string BaseUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange";
    private const string SourceName = "NBU";

    private readonly HttpClient _httpClient;

    public NbuExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = $"{BaseUrl}?date={date:yyyyMMdd}&json";
            var response = await _httpClient.GetFromJsonAsync<List<NbuExchangeRateResponse>>(url, cancellationToken);

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
