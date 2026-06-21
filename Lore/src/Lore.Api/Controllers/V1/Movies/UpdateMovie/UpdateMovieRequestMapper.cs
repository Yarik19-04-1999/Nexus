using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

public static class UpdateMovieRequestMapper
{
    public static UpdateMovieInput Map(int id, UpdateMovieRequest request)
        => new(
            id,
            request.Title,
            request.ReleaseYear,
            request.DurationMinutes,
            request.ReviewText,
            request.Score,
            request.ViewCount,
            request.RewatchStatus,
            request.UniverseId,
            request.ListNo
        );
}
