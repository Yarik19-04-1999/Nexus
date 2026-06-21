using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Application.Models.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateMovieMapper
{
    [MapperIgnoreSource(nameof(UpdateMovieInput.Id))]
    public static partial void ApplyUpdate(UpdateMovieInput input, Movie movie);
}
