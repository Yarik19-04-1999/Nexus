using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates.Dtos;
using Information.Application.Enums;
using Information.Application.Models;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;

public static class GetExchangeRatesResponseMapper
{
    public static GetExchangeRatesResponse Map(IReadOnlyDictionary<ExchangeCurrency, ExchangeRate> rates) =>
        new(rates.Select(kvp => new GetExchangeRatesItem(kvp.Key.ToString(), MapRate(kvp.Value))).ToList());

    private static ExchangeRateDto MapRate(ExchangeRate rate) =>
        new(rate.Rate, rate.Date);
}
