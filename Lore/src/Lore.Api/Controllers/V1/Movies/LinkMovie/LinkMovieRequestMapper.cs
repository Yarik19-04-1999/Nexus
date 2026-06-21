using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.LinkMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class LinkMovieRequestMapper
{
    public static partial LinkMovieToUniverseInput Map(LinkMovieRequest request);
}
