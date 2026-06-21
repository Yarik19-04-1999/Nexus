using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Application.Models.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateMovieMapper
{
    public static partial Movie Map(CreateMovieInput input);
}
