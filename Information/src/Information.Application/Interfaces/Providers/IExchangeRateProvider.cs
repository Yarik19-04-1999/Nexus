using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IExchangeRateProvider
{
    Task<IReadOnlyList<ExchangeRate>> GetUsdRates(CancellationToken cancellationToken = default);
}
