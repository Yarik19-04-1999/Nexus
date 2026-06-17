using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

public static class CreateUniverseResponseMapper
{
    public static CreateUniverseResponse Map(Universe universe)
        => new(
            universe.Id,
            universe.Name,
            universe.Description,
            universe.IsHidden,
            universe.ListNo,
            universe.CreatedAt,
            universe.UpdatedAt
        );
}
