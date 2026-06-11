using Dvizh.Application.DbContexts;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class ResetInviteAnswerUseCase : IResetInviteAnswerUseCase
{
    private readonly DvizhDbContext _context;

    public ResetInviteAnswerUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Execute(ResetInviteAnswerInput input, CancellationToken cancellationToken = default)
    {
        var affected = await _context.Invites
            .Where(x => x.Id == input.Id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(x => x.Answer, InviteAnswer.Pending)
                .SetProperty(x => x.UpdatedAt, DateTime.UtcNow),
                cancellationToken);

        if (affected == 0)
        {
            return ResultConstants.NotFound<Invite>(input.Id);
        }

        _context.InviteEvents.Add(new InviteEvent
        {
            InviteId = input.Id,
            EventType = InviteEventType.Reset
        });

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
