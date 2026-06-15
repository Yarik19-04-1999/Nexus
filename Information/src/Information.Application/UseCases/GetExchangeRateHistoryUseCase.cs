using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
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
            : Enum.GetValues<ExchangeCurrency>();

        var today = DateOnlyUtils.CurrentDate;
        var dates = new[]
        {
            today,
            today.Yesterday(),
            today.WeekAgo(),
            today.MonthAgo(),
            today.YearAgo(),
        };

        var tasks = dates.Select(date => _exchangeRateProvider.GetRates(date, cancellationToken));
        var results = await Task.WhenAll(tasks);

        var histories = currencies
            .Select(currency =>
            {
                var current = results[0].GetValueOrDefault(currency);
                if (current is null)
                {
                    return null;
                }

                return new ExchangeRateHistory
                {
                    Currency = currency,
                    Current = current,
                    Yesterday = results[1].GetValueOrDefault(currency),
                    WeekAgo = results[2].GetValueOrDefault(currency),
                    MonthAgo = results[3].GetValueOrDefault(currency),
                    YearAgo = results[4].GetValueOrDefault(currency),
                };
            })
            .Where(h => h is not null)
            .Cast<ExchangeRateHistory>()
            .ToList();

        if (histories.Count == 0)
        {
            throw InformationExceptions.ProviderUnavailable(nameof(IExchangeRateProvider));
        }

        return histories;
    }
}
