using Information.Application.Enums;
using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IExchangeRateProvider
{
    Task<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>> GetRates(DateOnly date, CancellationToken cancellationToken = default);
}
