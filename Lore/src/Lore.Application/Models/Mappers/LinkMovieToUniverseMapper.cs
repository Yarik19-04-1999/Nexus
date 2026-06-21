using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Application.Models.Mappers;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class LinkMovieToUniverseMapper
{
    [MapperIgnoreSource(nameof(LinkMovieToUniverseInput.MovieId))]
    public static partial void Map(LinkMovieToUniverseInput input, Movie movie);
}
