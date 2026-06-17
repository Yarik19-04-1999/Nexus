using Information.Application.Interfaces.Services;

namespace Information.Integration.Tests.TestDoubles;

/// <summary>
/// Simple test double for ICacheService that stores entries in a dictionary.
/// Used in caching provider tests to verify the decorator pattern without
/// requiring the real MemoryCache infrastructure.
/// </summary>
internal class InMemoryCacheService : ICacheService
{
    private readonly Dictionary<string, object?> _store = new();

    public async Task<T> GetOrCreate<T>(
        string key,
        Func<CancellationToken, Task<T>> factory,
        TimeSpan duration,
        CancellationToken cancellationToken = default)
    {
        if (_store.TryGetValue(key, out var cached))
        {
            return (T)cached!;
        }

        var value = await factory(cancellationToken);
        _store[key] = value;
        return value;
    }
}
