using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Application.Models.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateUniverseMapper
{
    public static partial Universe Map(CreateUniverseInput input);
}
