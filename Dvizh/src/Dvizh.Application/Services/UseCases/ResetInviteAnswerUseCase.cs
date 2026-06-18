using Dvizh.Application.DbContexts;
using Dvizh.Application.Enums;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Utils;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;
using Nexus.Infrastructure.EfCore.SqlServer.Extensions;

namespace Dvizh.Application.Services.UseCases;

public class ResetInviteAnswerUseCase : IResetInviteAnswerUseCase
{
    private readonly DvizhDbContext _context;

    public ResetInviteAnswerUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public Task<Result> Execute(ResetInviteAnswerInput input, CancellationToken cancellationToken = default)
        => _context.Database.ExecuteInTransaction(async () =>
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

            _context.InviteEvents.Add(EventUtils.CreateEvent(input.Id, InviteEventType.Reset));

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }, cancellationToken);
}
