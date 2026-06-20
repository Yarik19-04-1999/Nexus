using Dvizh.Application.DbContexts;
using Nexus.Infrastructure.Sieve.Extensions;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Models;
using Sieve.Services;

namespace Dvizh.Application.Services.UseCases;

public class GetInvitesUseCase : IGetInvitesUseCase
{
    private readonly DvizhDbContext _context;
    private readonly ISieveProcessor _sieve;

    public GetInvitesUseCase(DvizhDbContext context, ISieveProcessor sieve)
    {
        _context = context;
        _sieve = sieve;
    }

    public async Task<Result<PagedResult<Invite>>> Execute(GetInvitesInput input, CancellationToken cancellationToken = default)
    {
        var query = _context.Invites.AsNoTracking();
        var result = await _sieve.ToPagedResultAsync(input.SieveModel, query, cancellationToken);

        return Result<PagedResult<Invite>>.Success(result);
    }
}
