using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
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
    public async Task<IActionResult> GetFreeGames(
        [FromServices] IGetEpicFreeGamesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetEpicFreeGamesInput(), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }

        return Ok(new GetEpicFreeGamesResponse(result.Data.Select(GetEpicFreeGamesResponseMapper.Map).ToList()));
    }
}
