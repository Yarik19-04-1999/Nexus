using Dvizh.Application.Constants;
using Dvizh.Application.Interfaces;
using NanoidDotNet;

namespace Dvizh.Application.Services;

public class CodeGenerator : ICodeGenerator
{
    private const string Alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";

    public string Generate()
        => Nanoid.Generate(Alphabet, InviteConstants.CodeMaxLength);
}
