using Information.Application.Interfaces.Providers;
using Information.Application.Models;

namespace Information.Infrastructure.Providers;

public class ExchangeRateProvider : IExchangeRateProvider
{
    public Task<IReadOnlyList<ExchangeRate>> GetUsdRates(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
