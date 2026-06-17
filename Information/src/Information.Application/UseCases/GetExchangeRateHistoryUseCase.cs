using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Exceptions;
using Nexus.Application.Core.Extensions;
using Nexus.Application.Core.Utils;

namespace Information.Application.UseCases;

public class GetExchangeRateHistoryUseCase : IGetExchangeRateHistoryUseCase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRateHistoryUseCase(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<IReadOnlyList<ExchangeRateHistory>> Execute(GetExchangeRateHistoryInput input, CancellationToken cancellationToken = default)
    {
        var currencies = input.Currency.HasValue
            ? (IEnumerable<ExchangeCurrency>)[input.Currency.Value]
            : ExchangeCurrencies.All;

        var today = DateOnlyUtils.CurrentDate;
        var yesterday = today.Yesterday();
        var weekAgo = today.WeekAgo();
        var monthAgo = today.MonthAgo();
        var yearAgo = today.YearAgo();

        var ratesByDate = await _exchangeRateProvider.GetRates(
            [today, yesterday, weekAgo, monthAgo, yearAgo],
            cancellationToken);

        var todayRates = ratesByDate.GetValueOrDefault(today);
        var yesterdayRates = ratesByDate.GetValueOrDefault(yesterday);
        var weekAgoRates = ratesByDate.GetValueOrDefault(weekAgo);
        var monthAgoRates = ratesByDate.GetValueOrDefault(monthAgo);
        var yearAgoRates = ratesByDate.GetValueOrDefault(yearAgo);

        var histories = currencies
            .Select(currency =>
            {
                var current = todayRates?.GetValueOrDefault(currency);
                if (current is null)
                {
                    return null;
                }

                return new ExchangeRateHistory
                {
                    Currency = currency,
                    Current = current,
                    Yesterday = yesterdayRates?.GetValueOrDefault(currency),
                    WeekAgo = weekAgoRates?.GetValueOrDefault(currency),
                    MonthAgo = monthAgoRates?.GetValueOrDefault(currency),
                    YearAgo = yearAgoRates?.GetValueOrDefault(currency),
                };
            })
            .Where(h => h is not null)
            .Cast<ExchangeRateHistory>()
            .ToList();

        if (histories.Count == 0)
        {
            throw CommonExceptions.ExternalProviderNoData();
        }

        return histories;
    }
}
