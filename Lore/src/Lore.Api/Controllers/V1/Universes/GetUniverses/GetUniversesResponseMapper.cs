using Lore.Application.Models;
using Nexus.Application.Core.Models;

namespace Lore.Api.Controllers.V1.Universes.GetUniverses;

public static class GetUniversesResponseMapper
{
    public static GetUniversesResponse Map(PagedResult<Universe> result)
        => new(
            result.Items.Select(Map).ToList(),
            result.TotalCount,
            result.Page,
            result.PageSize,
            result.TotalPages
        );

    private static GetUniverseItemResponse Map(Universe universe)
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
