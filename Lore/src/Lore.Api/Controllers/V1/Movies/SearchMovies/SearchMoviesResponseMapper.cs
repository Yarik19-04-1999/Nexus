using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.SearchMovies;

public record SearchMovieItem(int Id, string Title, int ReleaseYear);

public static class SearchMoviesResponseMapper
{
    public static IReadOnlyList<SearchMovieItem> Map(IReadOnlyList<SearchMovieResult> results)
        => results.Select(x => new SearchMovieItem(x.Id, x.Title, x.ReleaseYear)).ToList();
}
