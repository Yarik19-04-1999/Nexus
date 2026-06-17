namespace Lore.Api.Controllers.V1.Universes.GetUniverseById;

public record GetUniverseByIdResponse(
    int Id,
    string Name,
    string? Description,
    bool IsHidden,
    int ListNo,
    DateTime CreatedAt,
    DateTime UpdatedAt
);
