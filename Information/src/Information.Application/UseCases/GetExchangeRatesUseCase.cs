using Information.Application.Constants;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Information.Application.Services;
using Nexus.Application.Core.Models;

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
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
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
