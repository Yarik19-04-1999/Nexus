using Dvizh.Application.DbContexts;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class DeleteInviteUseCase : IDeleteInviteUseCase
{
    private readonly DvizhDbContext _context;

    public DeleteInviteUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result> Execute(DeleteInviteInput input, CancellationToken cancellationToken = default)
    {
        var affected = await _context.Invites
            .Where(x => x.Id == input.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (affected == 0)
        {
            return ResultConstants.NotFound<Invite>(input.Id);
        }

        return Result.Success();
    }
}
