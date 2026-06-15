using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRateHistory;
using Information.Api.Controllers.V1.ExchangeRates.GetExchangeRates;
using Information.Application.Enums;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;

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
        var data = await useCase.Execute(new GetExchangeRatesInput(), cancellationToken);
        return Ok(GetExchangeRatesResponseMapper.Map(data));
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetAllHistory(
        [FromServices] IGetExchangeRateHistoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var data = await useCase.Execute(new GetExchangeRateHistoryInput(null), cancellationToken);
        return Ok(GetExchangeRateHistoryResponseMapper.Map(data));
    }

    [HttpGet("{currency}/history")]
    public async Task<IActionResult> GetHistory(
        ExchangeCurrency currency,
        [FromServices] IGetExchangeRateHistoryUseCase useCase,
        CancellationToken cancellationToken)
    {
        var data = await useCase.Execute(new GetExchangeRateHistoryInput(currency), cancellationToken);
        return Ok(GetExchangeRateHistoryResponseMapper.Map(data));
    }
}
