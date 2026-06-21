using Lore.Api.Controllers.V1.Universes.CreateUniverse;
using Lore.Api.Controllers.V1.Universes.GetUniverseById;
using Lore.Api.Controllers.V1.Universes.GetUniverses;
using Lore.Api.Controllers.V1.Universes.SearchUniverses;
using Lore.Api.Controllers.V1.Universes.UpdateUniverse;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;
using Sieve.Models;

namespace Lore.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class UniversesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<GetUniversesResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] SieveModel sieveModel,
        [FromServices] IGetUniversesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetUniversesInput(sieveModel), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetUniversesResponseMapper.Map(result.Data));
    }

    [HttpGet("search")]
    [ProducesResponseType<IReadOnlyList<SearchUniverseItem>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromServices] ISearchUniversesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new SearchUniversesInput(q), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(SearchUniversesResponseMapper.Map(result.Data));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<GetUniverseByIdResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetUniverseByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetUniverseByIdInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetUniverseByIdResponseMapper.Map(result.Data));
    }

    [HttpPost]
    [ProducesResponseType<CreateUniverseResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateUniverseRequest request,
        [FromServices] ICreateUniverseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(CreateUniverseRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, CreateUniverseResponseMapper.Map(result.Data));
    }

    [HttpPut]
    [ProducesResponseType<UpdateUniverseResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateUniverseRequest request,
        [FromServices] IUpdateUniverseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(UpdateUniverseRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(UpdateUniverseResponseMapper.Map(result.Data));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteUniverseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new DeleteUniverseInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return NoContent();
    }
}
