using Dvizh.Application.DbContexts;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Microsoft.EntityFrameworkCore;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class GetInviteByIdUseCase : IGetInviteByIdUseCase
{
    private readonly DvizhDbContext _context;

    public GetInviteByIdUseCase(DvizhDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Invite>> ExecuteAsync(GetInviteByIdInput input, CancellationToken cancellationToken = default)
    {
        var invite = await _context.Invites
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == input.Id, cancellationToken);

        if (invite is null)
        {
            return ResultConstants.NotFound<Invite>(input.Id);
        }

        return Result<Invite>.Success(invite);
    }
}
