using Dvizh.Api.Controllers.V1.Invites.CreateInvite;
using Dvizh.Api.Controllers.V1.Invites.GetInviteById;
using Dvizh.Api.Controllers.V1.Invites.OpenInvite;
using Dvizh.Api.Controllers.V1.Invites.RespondToInvite;
using Dvizh.Api.Controllers.V1.Invites.UpdateInvite;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models.Input;
using Microsoft.AspNetCore.Mvc;
using Nexus.Api.Core.Attributes;
using Nexus.Api.Core.Extensions;

namespace Dvizh.Api.Controllers.V1;

[ApiController]
[NexusRoute]
public class InviteController : ControllerBase
{
    [HttpGet("{id:int}")]
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

    [HttpPost]
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
