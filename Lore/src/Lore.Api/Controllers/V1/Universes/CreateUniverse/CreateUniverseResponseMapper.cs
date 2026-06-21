using Lore.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CreateUniverseResponseMapper
{
    public static partial CreateUniverseResponse Map(Universe universe);
}
