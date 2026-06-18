using Dvizh.Application.DbContexts;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Mappers;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class CreateInviteUseCase : ICreateInviteUseCase
{
    private readonly DvizhDbContext _context;
    private readonly IUniqueCodeService _uniqueCodeService;

    public CreateInviteUseCase(DvizhDbContext context, IUniqueCodeService uniqueCodeService)
    {
        _context = context;
        _uniqueCodeService = uniqueCodeService;
    }

    public async Task<Result<Invite>> Execute(CreateInviteInput input, CancellationToken cancellationToken = default)
    {
        var code = await _uniqueCodeService.GenerateUniqueCode(
            _context.Invites.CodeExists,
            cancellationToken);

        var invite = InviteMapper.MapCreate(input);
        invite.Code = code;

        _context.Invites.Add(invite);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Invite>.Success(invite);
    }
}
