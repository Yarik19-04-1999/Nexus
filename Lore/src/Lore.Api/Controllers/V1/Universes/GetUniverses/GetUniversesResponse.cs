namespace Lore.Api.Controllers.V1.Universes.GetUniverses;

public record GetUniversesResponse(
    IReadOnlyList<GetUniverseItemResponse> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record GetUniverseItemResponse(
    int Id,
    string Name,
    string? Description,
    bool IsHidden,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
