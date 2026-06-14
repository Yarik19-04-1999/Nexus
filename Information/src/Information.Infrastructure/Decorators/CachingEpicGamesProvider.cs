using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.Services;
using Information.Application.Models;
using Information.Application.Models.Options;
using Microsoft.Extensions.Options;
using Nexus.Application.Core.Models;

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

    public Task<Result<IReadOnlyList<EpicGame>>> GetFreeGames(CancellationToken cancellationToken = default) =>
        _cacheService.GetOrCreate(_cacheKeyProvider.GetFreeGamesKey(), ct => _inner.GetFreeGames(ct), _cacheOptions.CacheExpiration, cancellationToken);
}
