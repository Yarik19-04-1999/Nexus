using Dvizh.Application.DbContexts;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;
using Sieve.Services;

namespace Dvizh.Application.Services.UseCases;

public class GetInviteEventsUseCase : IGetInviteEventsUseCase
{
    private readonly DvizhDbContext _context;
    private readonly ISieveProcessor _sieve;

    public GetInviteEventsUseCase(DvizhDbContext context, ISieveProcessor sieve)
    {
        _context = context;
        _sieve = sieve;
    }

    public async Task<Result<PagedResult<InviteEvent>>> Execute(GetInviteEventsInput input, CancellationToken cancellationToken = default)
    {
        var inviteExists = await _context.Invites.AnyAsync(x => x.Id == input.InviteId, cancellationToken);
        if (!inviteExists)
        {
            return ResultConstants.NotFound<Invite>(input.InviteId);
        }

        var query = _context.InviteEvents.AsNoTracking().Where(x => x.InviteId == input.InviteId);

        var total = await _sieve.Apply(input.SieveModel, query, applyPagination: false).CountAsync(cancellationToken);
        var items = await _sieve.Apply(input.SieveModel, query).ToListAsync(cancellationToken);

        return Result<PagedResult<InviteEvent>>.Success(new PagedResult<InviteEvent>(
            items,
            total,
            input.SieveModel.Page ?? 1,
            input.SieveModel.PageSize ?? 20));
    }
}
