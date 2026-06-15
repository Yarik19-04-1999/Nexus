using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;

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
        var data = await useCase.Execute(new GetEpicFreeGamesInput(), cancellationToken);
        return Ok(new GetEpicFreeGamesResponse(data.Select(GetEpicFreeGamesResponseMapper.Map).ToList()));
    }
}
