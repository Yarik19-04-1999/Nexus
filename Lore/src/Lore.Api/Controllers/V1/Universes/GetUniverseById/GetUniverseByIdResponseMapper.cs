using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Universes.GetUniverseById;

public static class GetUniverseByIdResponseMapper
{
    public static GetUniverseByIdResponse Map(Universe universe)
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
