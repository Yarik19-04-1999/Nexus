using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.UnlinkMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UnlinkMovieRequestMapper
{
    public static partial UnlinkMovieFromUniverseInput Map(UnlinkMovieRequest request);
}
