using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.UpdateUniverse;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateUniverseRequestMapper
{
    public static partial UpdateUniverseInput Map(UpdateUniverseRequest request);
}
