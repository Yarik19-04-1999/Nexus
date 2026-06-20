using Dvizh.Api.Controllers.V1.Invites.CreateInvite;
using Nexus.Api.Core.ViewModels;
using Dvizh.Api.Controllers.V1.Invites.GetInviteById;
using Dvizh.Api.Controllers.V1.Invites.GetInviteEvents;
using Dvizh.Api.Controllers.V1.Invites.GetInvites;
using Dvizh.Api.Controllers.V1.Invites.OpenInvite;
using Dvizh.Api.Controllers.V1.Invites.RespondToInvite;
using Dvizh.Api.Controllers.V1.Invites.UpdateInvite;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;
using Sieve.Models;
using Dvizh.Api.Controllers.V1.Invites.GetInviteEvents.Dtos;
using Dvizh.Api.Controllers.V1.Invites.GetInvites.Dtos;

namespace Dvizh.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class InvitesController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType<PagedResponse<GetInviteDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromQuery] SieveModel sieveModel,
        [FromServices] IGetInvitesUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetInvitesInput(sieveModel), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetInvitesResponseMapper.Map(result.Data));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<GetInviteByIdResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetById(
        int id,
        [FromServices] IGetInviteByIdUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetInviteByIdInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetInviteByIdResponseMapper.Map(result.Data));
    }

    [HttpGet("{id:int}/events")]
    [ProducesResponseType<PagedResponse<GetInviteEventDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEvents(
        int id,
        [FromQuery] SieveModel sieveModel,
        [FromServices] IGetInviteEventsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new GetInviteEventsInput(id, sieveModel), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(GetInviteEventsResponseMapper.Map(result.Data));
    }

    [HttpPost]
    [ProducesResponseType<CreateInviteResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create(
        [FromBody] CreateInviteRequest request,
        [FromServices] ICreateInviteUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(CreateInviteRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, CreateInviteResponseMapper.Map(result.Data));
    }

    [HttpPut]
    [ProducesResponseType<UpdateInviteResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(
        [FromBody] UpdateInviteRequest request,
        [FromServices] IUpdateInviteUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(UpdateInviteRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(UpdateInviteResponseMapper.Map(result.Data));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(
        int id,
        [FromServices] IDeleteInviteUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new DeleteInviteInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return NoContent();
    }

    [HttpPost("{id:int}/answer/reset")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> ResetAnswer(
        int id,
        [FromServices] IResetInviteAnswerUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new ResetInviteAnswerInput(id), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return NoContent();
    }

    [HttpGet("{code}")]
    [ProducesResponseType<OpenInviteResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> Open(
        string code,
        [FromServices] IOpenInviteUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(new OpenInviteInput(code), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return Ok(OpenInviteResponseMapper.Map(result.Data));
    }

    [HttpPost("answer")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Respond(
        [FromBody] RespondToInviteRequest request,
        [FromServices] IRespondToInviteUseCase useCase,
        CancellationToken cancellationToken)
    {
        var result = await useCase.Execute(RespondToInviteRequestMapper.Map(request), cancellationToken);
        if (result.HasError)
        {
            return this.DomainError(result);
        }
        return NoContent();
    }
}
