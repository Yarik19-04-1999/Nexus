using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.CreateUniverse;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateUniverseRequestMapper
{
    public static partial CreateUniverseInput Map(CreateUniverseRequest request);
}
