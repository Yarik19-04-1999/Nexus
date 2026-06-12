using System.Collections.Concurrent;
using Information.Application.Interfaces.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Information.Infrastructure.Services;

internal class CacheService(IMemoryCache memoryCache) : ICacheService
{
    private readonly IMemoryCache _memoryCache = memoryCache;
    private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores = new();

    public async Task<T> GetOrCreate<T>(string key, Func<CancellationToken, Task<T>> factory, TimeSpan duration, CancellationToken cancellationToken = default)
    {
        if (_memoryCache.TryGetValue(key, out T? cached) && cached is not null)
        {
            return cached;
        }

        var semaphore = _semaphores.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));

        await semaphore.WaitAsync(cancellationToken);
        try
        {
            if (_memoryCache.TryGetValue(key, out cached) && cached is not null)
            {
                return cached;
            }

            var value = await factory(cancellationToken);
            _memoryCache.Set(key, value, duration);
            return value;
        }
        finally
        {
            semaphore.Release();
        }
    }
}
