using Dvizh.Application.DbContexts;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class UpdateInviteUseCase : IUpdateInviteUseCase
{
    private readonly DvizhDbContext _context;

    public UpdateInviteUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Invite>> Execute(UpdateInviteInput input, CancellationToken cancellationToken = default)
    {
        var invite = await _context.Invites.FindById(input.Id, cancellationToken);

        if (invite is null)
        {
            return ResultConstants.NotFound<Invite>(input.Id);
        }

        invite.Message = input.Message;
        invite.Description = input.Description;
        invite.ExpiresAt = input.ExpiresAt;
        invite.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Invite>.Success(invite);
    }
}
