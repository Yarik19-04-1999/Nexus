using Dvizh.Application.Constants;
using Dvizh.Application.Interfaces;
using NanoidDotNet;

namespace Dvizh.Application.Services;

public class InviteCodeGenerator : IInviteCodeGenerator
{
    public string Generate()
        => Nanoid.Generate(CodeGeneratorConstants.Alphabet, InviteValidationConstants.Invite.CodeMaxLength);
}
