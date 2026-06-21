using Lore.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class UpdateUniverseResponseMapper
{
    public static partial UpdateUniverseResponse Map(Universe universe);
}
