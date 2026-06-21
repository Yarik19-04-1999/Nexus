using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.GetMovieById;

public record GetMovieByIdResponse(
    int Id,
    string Title,
    int ReleaseYear,
    int DurationMinutes,
    string? ReviewText,
    decimal? Score,
    int ViewCount,
    RewatchStatus RewatchStatus,
    int? UniverseId,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
