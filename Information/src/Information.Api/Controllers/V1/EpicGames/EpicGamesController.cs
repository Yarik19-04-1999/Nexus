using Information.Api.Controllers.V1.EpicGames.GetEpicGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;

namespace Information.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class EpicGamesController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetEpicGames(
        [FromServices] IGetEpicGamesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetEpicGamesInput(), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(new GetEpicGamesResponse(result.Data.Select(GetEpicGamesResponseMapper.Map).ToList()));
    }
}
