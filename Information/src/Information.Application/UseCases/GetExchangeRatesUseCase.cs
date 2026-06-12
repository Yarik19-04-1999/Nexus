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

    public async Task<Result<IReadOnlyList<ExchangeRate>>> Execute(GetExchangeRatesInput input, CancellationToken cancellationToken = default)
    {
        var rates = await _provider.GetUsdRates(cancellationToken);
        return Result<IReadOnlyList<ExchangeRate>>.Success(rates);
    }
}
