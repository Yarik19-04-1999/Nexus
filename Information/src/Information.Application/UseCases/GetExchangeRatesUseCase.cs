using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
{
    private readonly IExchangeRateProvider _provider;

    public GetExchangeRatesUseCase(IExchangeRateProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken = default)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var result = await _provider.GetRates(today, cancellationToken);

        if (result.HasError)
        {
            return result;
        }

        if (result.Data.Count == 0)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>(nameof(IExchangeRateProvider));
        }

        return result;
    }
}
