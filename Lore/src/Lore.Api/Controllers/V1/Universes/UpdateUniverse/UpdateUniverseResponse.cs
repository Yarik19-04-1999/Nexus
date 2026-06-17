namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

public record UpdateUniverseResponse(
    int Id,
    string Name,
    string? Description,
    bool IsHidden,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
