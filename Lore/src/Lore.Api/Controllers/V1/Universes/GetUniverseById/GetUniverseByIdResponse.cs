using Lore.Application.Models.Enums;

namespace Lore.Api.Controllers.V1.Universes.GetUniverseById;

public record GetUniverseByIdResponse(
    int Id,
    string Name,
    string? Description,
    bool IsHidden,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<MovieInUniverseItem> Movies
);

public record MovieInUniverseItem(
    int Id,
    string Title,
    int ReleaseYear,
    int DurationMinutes,
    string? ReviewText,
    decimal? Score,
    int ViewCount,
    RewatchStatus RewatchStatus
);
