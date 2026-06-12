using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Extensions;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetExchangeRateHistoryUseCase : IGetExchangeRateHistoryUseCase
{
    private readonly IExchangeRateProvider _provider;

    public GetExchangeRateHistoryUseCase(IExchangeRateProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<IReadOnlyList<ExchangeRateHistory>>> Execute(GetExchangeRateHistoryInput input, CancellationToken cancellationToken = default)
    {
        var currencies = input.Currency.HasValue
            ? (IEnumerable<ExchangeCurrency>)[input.Currency.Value]
            : Enum.GetValues<ExchangeCurrency>();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var dates = new[]
        {
            today,
            today.Yesterday(),
            today.WeekAgo(),
            today.MonthAgo(),
            today.YearAgo(),
        };

        var tasks = dates.Select(date => _provider.GetRates(date, cancellationToken));
        var results = await Task.WhenAll(tasks);

        foreach (var result in results)
        {
            if (result.HasError)
            {
                return InformationResultConstants.ProviderUnavailable<IReadOnlyList<ExchangeRateHistory>>(nameof(IExchangeRateProvider));
            }
        }

        var histories = currencies
            .Select(currency =>
            {
                var current = results[0].Data.GetValueOrDefault(currency);
                if (current is null)
                {
                    return null;
                }

                return new ExchangeRateHistory
                {
                    Currency = currency,
                    Current = current,
                    Yesterday = results[1].Data.GetValueOrDefault(currency),
                    WeekAgo = results[2].Data.GetValueOrDefault(currency),
                    MonthAgo = results[3].Data.GetValueOrDefault(currency),
                    YearAgo = results[4].Data.GetValueOrDefault(currency),
                };
            })
            .Where(h => h is not null)
            .Cast<ExchangeRateHistory>()
            .ToList();

        if (histories.Count == 0)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<ExchangeRateHistory>>(nameof(IExchangeRateProvider));
        }

        return Result<IReadOnlyList<ExchangeRateHistory>>.Success(histories);
    }
}
