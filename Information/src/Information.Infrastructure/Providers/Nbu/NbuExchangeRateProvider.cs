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
    private const string SourceName = "NBU";

    private readonly HttpClient _httpClient;

    public NbuExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private static string FormUrl(DateOnly date) => $"/NBUStatService/v1/statdirectory/exchange?date={date:yyyyMMdd}&json";

    public async Task<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>> GetRates(DateOnly date, CancellationToken cancellationToken = default)
    {
        try
        {
            var url = FormUrl(date);
            var response = await _httpClient.GetFromJsonAsync<List<NbuExchangeRateResponse>>(url, cancellationToken);

            if (response is null)
            {
                throw InformationExceptions.ProviderUnavailable(SourceName);
            }

            var knownCurrencies = Enum.GetValues<ExchangeCurrency>()
                .ToDictionary(c => c.ToString(), c => c, StringComparer.OrdinalIgnoreCase);

            return response
                .Where(r => knownCurrencies.ContainsKey(r.Currency))
                .ToDictionary(
                    r => knownCurrencies[r.Currency],
                    r => new ExchangeRate { Rate = r.Rate, Date = date });
        }
        catch (DomainException)
        {
            throw;
        }
        catch (Exception)
        {
            throw InformationExceptions.ProviderUnavailable(SourceName);
        }
    }
}
