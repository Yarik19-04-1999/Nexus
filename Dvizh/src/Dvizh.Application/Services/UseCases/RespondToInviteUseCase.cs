using Dvizh.Application.Constants;
using Dvizh.Application.DbContexts;
using Dvizh.Application.Enums;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Extensions;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class RespondToInviteUseCase : IRespondToInviteUseCase
{
    private readonly DvizhDbContext _context;

    public RespondToInviteUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Execute(RespondToInviteInput input, CancellationToken cancellationToken = default)
    {
        var invite = await _context.Invites.FindByCode(input.Code, cancellationToken);

        if (invite is null)
        {
            return ResultConstants.NotFound<Invite>(input.Code);
        }

        if (invite.IsExpired())
        {
            return ResultConstants.AlreadyExpired<Invite>(invite.Id);
        }

        if (invite.Answer != InviteAnswer.Pending)
        {
            return DvizhResultConstants.AlreadyAnswered(invite.Id);
        }

        invite.Answer = input.Answer;
        invite.UpdatedAt = DateTime.UtcNow;

        _context.InviteEvents.Add(EventUtils.CreateEvent(invite, input.Answer.ToEventType()));

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
