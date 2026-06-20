namespace Dvizh.Application.Interfaces;

public interface IUniqueCodeService
{
    Task<string> GenerateUniqueCode(
        Func<string> generate,
        Func<string, CancellationToken, Task<bool>> existsAsync,
        CancellationToken cancellationToken = default);
}
