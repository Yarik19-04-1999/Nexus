using Lore.Api.Controllers.V1.Movies.CreateMovie;
using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Lore.Api.Controllers.V1.Movies.GetMovies;
using Lore.Api.Controllers.V1.Movies.LinkMovie;
using Lore.Api.Controllers.V1.Movies.SearchMovies;
using Lore.Api.Controllers.V1.Movies.UnlinkMovie;
using Lore.Api.Controllers.V1.Movies.UpdateMovie;
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
public class MoviesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<GetMoviesResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] SieveModel sieveModel,
        [FromServices] IGetMoviesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetMoviesInput(sieveModel), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetMoviesResponseMapper.Map(result.Data));
    }

    [HttpGet("search")]
    [ProducesResponseType<IReadOnlyList<SearchMovieItem>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search(
        [FromQuery] string q,
        [FromServices] ISearchMoviesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new SearchMoviesInput(q), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(SearchMoviesResponseMapper.Map(result.Data));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<GetMovieByIdResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetMovieByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetMovieByIdInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetMovieByIdResponseMapper.Map(result.Data));
    }

    [HttpPost]
    [ProducesResponseType<GetMovieByIdResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateMovieRequest request,
        [FromServices] ICreateMovieUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(CreateMovieRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, CreateMovieResponseMapper.Map(result.Data));
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<GetMovieByIdResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateMovieRequest request,
        [FromServices] IUpdateMovieUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(UpdateMovieRequestMapper.Map(id, request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(UpdateMovieResponseMapper.Map(result.Data));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteMovieUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new DeleteMovieInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return NoContent();
    }

    [HttpPost("link")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Link(
        [FromBody] LinkMovieRequest request,
        [FromServices] ILinkMovieToUniverseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(LinkMovieRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok();
    }

    [HttpPost("unlink")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Unlink(
        [FromBody] UnlinkMovieRequest request,
        [FromServices] IUnlinkMovieFromUniverseUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(UnlinkMovieRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok();
    }
}
