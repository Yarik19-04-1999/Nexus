using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class GetMoviesUseCase : IGetMoviesUseCase
{
    private readonly ILoreStore _store;

    public GetMoviesUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<PagedResult<Movie>>> Execute(GetMoviesInput input, CancellationToken cancellationToken = default)
    {
        var result = await _store.GetMoviesPaged(input.Sieve, cancellationToken);
        return Result<PagedResult<Movie>>.Success(result);
    }
}
