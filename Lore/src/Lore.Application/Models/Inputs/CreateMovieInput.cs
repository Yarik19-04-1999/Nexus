using Lore.Application.Models.Enums;

namespace Lore.Application.Models.Inputs;

public record CreateMovieInput(
    string Title,
    int ReleaseYear,
    int DurationMinutes,
    string? ReviewText,
    decimal? Score,
    int ViewCount,
    RewatchStatus RewatchStatus,
    int? UniverseId,
    int ListNo);
