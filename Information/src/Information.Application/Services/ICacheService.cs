namespace Information.Application.Services;

public interface ICacheService
{
    Task<T> GetOrCreate<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan duration, CancellationToken cancellationToken = default);
}
