using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory.Dtos;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;

public record GetExchangeRateHistoryResponse(
    string Currency,
    ExchangeRatePointDto Current,
    ExchangeRatePointDto? Yesterday,
    ExchangeRatePointDto? WeekAgo,
    ExchangeRatePointDto? MonthAgo,
    ExchangeRatePointDto? YearAgo);
