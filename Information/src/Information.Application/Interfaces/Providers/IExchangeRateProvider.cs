using Information.Application.Enums;
using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IExchangeRateProvider
{
    Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> GetRates(DateOnly date, CancellationToken cancellationToken = default);
}
