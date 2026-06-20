using Nexus.Application.Core.Extensions;
using Nexus.Application.Core.Interfaces;

namespace Nexus.Application.Core.Services;

public class UniqueCodeService : IUniqueCodeService
{
    public async Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        CancellationToken cancellationToken = default)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var code = generate();
            if (!await existsAsync(code, cancellationToken))
            {
                return code;
            }
        }
        cancellationToken.ThrowIfCancellationRequested();
        throw new InvalidOperationException();
    }

    public Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        TimeSpan timeout,
        CancellationToken cancellationToken = default)
    {
        Func<CancellationToken, Task<string>> operation = ct => GenerateUniqueCode(generate, existsAsync, ct);
        return operation.ExecuteWithTimeout(timeout, cancellationToken);
    }
}
