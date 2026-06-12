using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates.Dtos;
using Information.Application.Enums;
using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetExchangeRatesResponseMapper
{
    public static GetExchangeRatesResponse Map(IReadOnlyDictionary<ExchangeCurrency, ExchangeRate> rates) =>
        new(rates.Select(kvp => new GetExchangeRatesItem(kvp.Key, Map(kvp.Value))).ToList());

    private static partial ExchangeRateDto Map(ExchangeRate rate);
}
