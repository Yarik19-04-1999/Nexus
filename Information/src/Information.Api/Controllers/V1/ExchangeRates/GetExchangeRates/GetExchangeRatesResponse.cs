using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates.Dtos;
using Information.Application.Enums;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;

public record GetExchangeRatesResponse(IReadOnlyList<GetExchangeRatesItem> Rates);

public record GetExchangeRatesItem(ExchangeCurrency Currency, ExchangeRateDto Rate);
