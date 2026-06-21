using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.GetMovieById;

public static class GetMovieByIdResponseMapper
{
    public static GetMovieByIdResponse Map(Movie movie)
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
