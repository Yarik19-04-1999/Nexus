using Information.Application.Enums;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Exceptions;
using Nexus.Application.Core.Utils;

namespace Information.Application.UseCases;

public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetExchangeRatesUseCase(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken = default)
    {
        var today = DateOnlyUtils.CurrentDate;
        var ratesByDate = await _exchangeRateProvider.GetRates([today], cancellationToken);

        if (!ratesByDate.TryGetValue(today, out var rates))
        {
            throw CommonExceptions.ExternalProviderNoData();
        }

        return rates;
    }
}
