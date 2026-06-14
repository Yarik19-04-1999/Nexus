using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Utils;

namespace Information.Application.UseCases;

public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRatesUseCase(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken = default)
    {
        var today = DateOnlyUtils.CurrentDate;
        var result = await _exchangeRateProvider.GetRates(today, cancellationToken);

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
