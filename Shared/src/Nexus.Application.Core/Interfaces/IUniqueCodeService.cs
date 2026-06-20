namespace Nexus.Application.Core.Interfaces;

public interface IUniqueCodeService
{
    Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        CancellationToken cancellationToken = default);

    Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        TimeSpan timeout,
        CancellationToken cancellationToken = default);
}
