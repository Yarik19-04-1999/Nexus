using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

public record UpdateMovieRequest(
    string Title,
    int ReleaseYear,
    int DurationMinutes,
    string? ReviewText,
    decimal? Score,
    int ViewCount,
    RewatchStatus RewatchStatus,
    int? UniverseId,
    int ListNo
);
