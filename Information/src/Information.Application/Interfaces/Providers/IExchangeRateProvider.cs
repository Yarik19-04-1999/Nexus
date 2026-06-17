using Information.Application.Enums;
using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IExchangeRateProvider
{
    Task<IReadOnlyDictionary<DateOnly, IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(IReadOnlyList<DateOnly> dates, CancellationToken cancellationToken = default);
}
