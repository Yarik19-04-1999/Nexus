using Lore.Application.Models.Results;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.SearchMovies;

public record SearchMovieItem(int Id, string Title, int ReleaseYear);

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class SearchMoviesResponseMapper
{
    public static partial SearchMovieItem Map(SearchMovieResult result);

    public static IReadOnlyList<SearchMovieItem> Map(IReadOnlyList<SearchMovieResult> results)
        => results.Select(Map).ToList();
}
