using Dvizh.Application.DbContexts;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Mappers;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Dvizh.Application.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class CreateInviteUseCase : ICreateInviteUseCase
{
    private readonly DvizhDbContext _context;
    private readonly IUniqueCodeService _uniqueCodeService;
    private readonly IInviteCodeGenerator _codeGenerator;
    private readonly InviteCodeGenerationOptions _options;

    public CreateInviteUseCase(
        DvizhDbContext context,
        IUniqueCodeService uniqueCodeService,
        IInviteCodeGenerator codeGenerator,
        IOptions<InviteCodeGenerationOptions> options)
    {
        _context = context;
        _uniqueCodeService = uniqueCodeService;
        _codeGenerator = codeGenerator;
        _options = options.Value;
    }

    public async Task<Result<Invite>> Execute(CreateInviteInput input, CancellationToken cancellationToken = default)
    {
        var code = await _uniqueCodeService.GenerateUniqueCode(
            _codeGenerator.Generate,
            _context.Invites.CodeExists,
            _options.Timeout,
            cancellationToken);

        var invite = InviteMapper.MapCreate(input);
        invite.Code = code;

        _context.Invites.Add(invite);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Invite>.Success(invite);
    }
}
