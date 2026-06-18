using Dvizh.Application.Interfaces;
using Dvizh.Application.Options;
using Microsoft.Extensions.Options;

namespace Dvizh.Application.Services;

public class UniqueCodeService : IUniqueCodeService
{
    private readonly IInviteCodeGenerator _generator;
    private readonly UniqueCodeServiceOptions _options;

    public UniqueCodeService(IInviteCodeGenerator generator, IOptions<UniqueCodeServiceOptions> options)
    {
        _generator = generator;
        _options = options.Value;
    }

    public async Task<string> GenerateUniqueCode(
        Func<string, CancellationToken, Task<bool>> existsAsync,
        CancellationToken cancellationToken = default)
    {
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(_options.Timeout);

        try
        {
            while (!cts.Token.IsCancellationRequested)
            {
                var code = _generator.Generate();
                if (!await existsAsync(code, cts.Token))
                {
                    return code;
                }
            }
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new InvalidOperationException(
                $"Failed to generate a unique code within {_options.Timeout.TotalSeconds}s.");
        }

        throw new InvalidOperationException(
            $"Failed to generate a unique code within {_options.Timeout.TotalSeconds}s.");
    }
}
