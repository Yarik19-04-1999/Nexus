using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

public static class UpdateUniverseResponseMapper
{
    public static UpdateUniverseResponse Map(Universe universe)
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
