using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.Services;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Utils;

namespace Information.Application.UseCases;

public class GetExchangeRatesUseCase : IGetExchangeRatesUseCase
{
    private readonly IExchangeRateService _service;

    public GetExchangeRatesUseCase(IExchangeRateService service)
    {
        _service = service;
    }

    public async Task<Result<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken = default)
    {
        var today = DateOnlyUtils.CurrentDate;
        var result = await _service.GetRates(today, cancellationToken);

        if (result.HasError)
        {
            return result;
        }

        if (result.Data.Count == 0)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyDictionary<ExchangeCurrency, ExchangeRate>>(nameof(IExchangeRateService));
        }

        return result;
    }
}
