using Dvizh.Application.DbContexts;
using Dvizh.Application.Enums;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class OpenInviteUseCase : IOpenInviteUseCase
{
    private readonly DvizhDbContext _context;

    public OpenInviteUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Invite>> Execute(OpenInviteInput input, CancellationToken cancellationToken = default)
    {
        var invite = await _context.Invites.FindByCode(input.Code, cancellationToken);

        if (invite is null)
        {
            return ResultConstants.NotFound<Invite>(input.Code);
        }

        _context.InviteEvents.Add(EventUtils.CreateEvent(invite, InviteEventType.Opened));

        await _context.SaveChangesAsync(cancellationToken);

        return Result<Invite>.Success(invite);
    }
}
