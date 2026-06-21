using Lore.Application.Models.Enums;

namespace Lore.Api.Controllers.V1.Movies.CreateMovie;

public record CreateMovieRequest(
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
