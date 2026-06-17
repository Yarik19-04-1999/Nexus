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
            : ExchangeCurrencyConstants.All;

        var today = DateOnlyUtils.CurrentDate;

        var todayTask = _exchangeRateProvider.GetRates(today, cancellationToken);
        var yesterdayTask = _exchangeRateProvider.GetRates(today.Yesterday(), cancellationToken);
        var weekAgoTask = _exchangeRateProvider.GetRates(today.WeekAgo(), cancellationToken);
        var monthAgoTask = _exchangeRateProvider.GetRates(today.MonthAgo(), cancellationToken);
        var yearAgoTask = _exchangeRateProvider.GetRates(today.YearAgo(), cancellationToken);

        await Task.WhenAll(todayTask, yesterdayTask, weekAgoTask, monthAgoTask, yearAgoTask);

        var histories = currencies
            .Select(currency =>
            {
                var current = todayTask.Result.GetValueOrDefault(currency);
                if (current is null)
                {
                    return null;
                }

                return new ExchangeRateHistory
                {
                    Currency = currency,
                    Current = current,
                    Yesterday = yesterdayTask.Result.GetValueOrDefault(currency),
                    WeekAgo = weekAgoTask.Result.GetValueOrDefault(currency),
                    MonthAgo = monthAgoTask.Result.GetValueOrDefault(currency),
                    YearAgo = yearAgoTask.Result.GetValueOrDefault(currency),
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
