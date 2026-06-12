using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates.Dtos;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;

public record GetExchangeRatesResponse(IReadOnlyList<GetExchangeRatesItem> Rates);

public record GetExchangeRatesItem(string Currency, ExchangeRateDto Rate);
