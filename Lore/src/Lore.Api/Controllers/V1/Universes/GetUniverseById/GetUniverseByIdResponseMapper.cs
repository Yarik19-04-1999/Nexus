using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.GetUniverseById;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetUniverseByIdResponseMapper
{
    public static partial GetUniverseByIdResponse Map(Universe universe);
    private static partial MovieInUniverseItem Map(Movie movie);
}
