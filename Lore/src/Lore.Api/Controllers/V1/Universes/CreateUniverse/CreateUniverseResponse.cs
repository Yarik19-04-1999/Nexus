namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

public record CreateUniverseResponse(
    int Id,
    string Name,
    string? Description,
    bool IsHidden,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
