using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;

namespace Information.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class ExchangeRateController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetRates(
        [FromServices] IGetExchangeRatesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetExchangeRatesInput(), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(GetExchangeRatesResponseMapper.Map(result.Data));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetAllHistory(
        [FromServices] IGetExchangeRateHistoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetExchangeRateHistoryInput(null), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(GetExchangeRateHistoryResponseMapper.Map(result.Data));
    }

    [HttpGet("{currency}/history")]
    public async Task<IActionResult> GetHistory(
        ExchangeCurrency currency,
        [FromServices] IGetExchangeRateHistoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetExchangeRateHistoryInput(currency), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(GetExchangeRateHistoryResponseMapper.Map(result.Data));
    }
}
