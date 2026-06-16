using Dvizh.Application.DbContexts;
using Dvizh.Application.Extensions;
using Dvizh.Application.Interfaces;
using Dvizh.Application.Interfaces.UseCases;
using Dvizh.Application.Models;
using Dvizh.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Dvizh.Application.Services.UseCases;

public class CreateInviteUseCase : ICreateInviteUseCase
{
    private readonly DvizhDbContext _context;
    private readonly IInviteCodeGenerator _codeGenerator;

    public CreateInviteUseCase(DvizhDbContext context, IInviteCodeGenerator codeGenerator)
    {
        _context = context;
        _codeGenerator = codeGenerator;
    }

    public async Task<Result<Invite>> Execute(CreateInviteInput input, CancellationToken cancellationToken = default)
    {
        string code;

        do
        {
            code = _codeGenerator.Generate();
        }
        while (await _context.Invites.CodeExists(code, cancellationToken));

        var invite = new Invite
        {
            Code = code,
            Message = input.Message,
            Description = input.Description,
            ExpiresAt = input.ExpiresAt,
            Language = input.Language,
            Mascot = input.Mascot,
        };

        _context.Invites.Add(invite);
        await _context.SaveChangesAsync(cancellationToken);

        return Result<Invite>.Success(invite);
    }
}
