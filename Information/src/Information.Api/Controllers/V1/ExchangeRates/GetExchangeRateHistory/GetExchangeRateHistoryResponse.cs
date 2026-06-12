using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory.Dtos;
using Information.Application.Enums;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;

public record GetExchangeRateHistoryResponse(
    ExchangeCurrency Currency,
    ExchangeRatePointDto Current,
    ExchangeRatePointDto? Yesterday,
    ExchangeRatePointDto? WeekAgo,
    ExchangeRatePointDto? MonthAgo,
    ExchangeRatePointDto? YearAgo);
