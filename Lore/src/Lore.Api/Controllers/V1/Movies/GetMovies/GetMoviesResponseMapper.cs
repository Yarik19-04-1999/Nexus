using Lore.Application.Models;
using Nexus.Application.Core.Models;

namespace Lore.Api.Controllers.V1.Movies.GetMovies;

public static class GetMoviesResponseMapper
{
    public static GetMoviesResponse Map(PagedResult<Movie> result)
        => new(
            result.Items.Select(Map).ToList(),
            result.TotalCount,
            result.Page,
            result.PageSize,
            result.TotalPages
        );

    private static GetMovieItemResponse Map(Movie movie)
        => new(
            movie.Id,
            movie.Title,
            movie.ReleaseYear,
            movie.DurationMinutes,
            movie.ReviewText,
            movie.Score,
            movie.ViewCount,
            movie.RewatchStatus,
            movie.UniverseId,
            movie.ListNo,
            movie.CreatedAt,
            movie.UpdatedAt
        );
}
