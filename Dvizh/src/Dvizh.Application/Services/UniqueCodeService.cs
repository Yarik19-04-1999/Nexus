using Dvizh.Application.Interfaces;
using Dvizh.Application.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Extensions;

namespace Dvizh.Application.Services;

public class UniqueCodeService : IUniqueCodeService
{
    private readonly UniqueCodeGeneratorOptions _options;

    public UniqueCodeService(IOptions<UniqueCodeGeneratorOptions> options)
    {
        _options = options.Value;
    }

    public Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        CancellationToken cancellationToken = default)
    {
        Func<CancellationToken, Task<string>> operation = async ct =>
        {
            while (!ct.IsCancellationRequested)
            {
                var code = generate();
                if (!await existsAsync(code, ct))
                {
                    return code;
                }
            }
            throw new InvalidOperationException("Operation was cancelled before a unique code could be generated.");
        };

        return operation.ExecuteWithTimeout(_options.Timeout, cancellationToken);
    }
}
