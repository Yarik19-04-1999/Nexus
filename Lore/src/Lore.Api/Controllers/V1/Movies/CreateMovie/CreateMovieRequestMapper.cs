using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Movies.CreateMovie;

public static class CreateMovieRequestMapper
{
    public static CreateMovieInput Map(CreateMovieRequest request)
        => new(
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
