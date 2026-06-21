using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Results;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class SearchMoviesUseCase : ISearchMoviesUseCase
{
    private readonly ILoreStore _store;

    public SearchMoviesUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<IReadOnlyList<SearchMovieResult>>> Execute(SearchMoviesInput input, CancellationToken cancellationToken = default)
    {
        var movies = await _store.SearchMovies(input.Query, cancellationToken);
        var results = movies.Select(x => new SearchMovieResult(x.Id, x.Title, x.ReleaseYear)).ToList();
        return Result<IReadOnlyList<SearchMovieResult>>.Success(results);
    }
}
