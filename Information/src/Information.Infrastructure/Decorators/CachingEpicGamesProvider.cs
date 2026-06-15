using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;

namespace Information.Infrastructure.Decorators;

internal class CachingEpicGamesProvider : IEpicGamesProvider
{
    private readonly IEpicGamesProvider _inner;
    private readonly ICacheService _cacheService;
    private readonly ICacheKeyProvider _cacheKeyProvider;
    private readonly EpicGamesCacheOptions _cacheOptions;

    public CachingEpicGamesProvider(IEpicGamesProvider inner, ICacheService cacheService, ICacheKeyProvider cacheKeyProvider, IOptions<EpicGamesCacheOptions> cacheOptions)
    {
        _inner = inner;
        _cacheService = cacheService;
        _cacheKeyProvider = cacheKeyProvider;
        _cacheOptions = cacheOptions.Value;
    }

    public async Task<IReadOnlyList<EpicGame>> GetFreeGames(CancellationToken cancellationToken = default) =>
        await _cacheService.GetOrCreate(_cacheKeyProvider.GetEpicGamesKey(), _inner.GetFreeGames, _cacheOptions.CacheExpiration, cancellationToken);
}
