using Lore.Application.Models.Enums;

namespace Lore.Api.Controllers.V1.Movies.GetMovies;

public record GetMoviesResponse(
    IReadOnlyList<GetMovieItemResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record GetMovieItemResponse(
    int Id,
    string Title,
    int ReleaseYear,
    int DurationMinutes,
    string? ReviewText,
    decimal? Score,
    int ViewCount,
    RewatchStatus RewatchStatus,
    int? UniverseId,
    string? UniverseName,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
