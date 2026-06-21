using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Application.Models.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateUniverseMapper
{
    [MapperIgnoreSource(nameof(UpdateUniverseInput.Id))]
    public static partial void ApplyUpdate(UpdateUniverseInput input, Universe universe);
}
