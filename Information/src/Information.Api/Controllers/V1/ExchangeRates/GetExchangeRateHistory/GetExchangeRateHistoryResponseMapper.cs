using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory.Dtos;
using Information.Application.Models;

namespace Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;

public static class GetExchangeRateHistoryResponseMapper
{
    public static IReadOnlyList<GetExchangeRateHistoryResponse> Map(IReadOnlyList<ExchangeRateHistory> histories) =>
        histories.Select(Map).ToList();

    private static GetExchangeRateHistoryResponse Map(ExchangeRateHistory history) =>
        new(
            history.Currency.ToString(),
            MapPoint(history.Current),
            history.Yesterday is not null ? MapPoint(history.Yesterday) : null,
            history.WeekAgo is not null ? MapPoint(history.WeekAgo) : null,
            history.MonthAgo is not null ? MapPoint(history.MonthAgo) : null,
            history.YearAgo is not null ? MapPoint(history.YearAgo) : null);

    private static ExchangeRatePointDto MapPoint(ExchangeRate rate) =>
        new(rate.Rate, rate.Date);
}
