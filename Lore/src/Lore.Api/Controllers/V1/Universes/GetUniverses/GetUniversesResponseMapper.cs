using Lore.Application.Models;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.GetUniverses;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetUniversesResponseMapper
{
    public static GetUniversesResponse Map(PagedResult<Universe> result)
        => new(result.Items.Select(Map).ToList(), result.TotalCount, result.Page, result.PageSize, result.TotalPages);

    public static partial GetUniverseItemResponse Map(Universe universe);
}
