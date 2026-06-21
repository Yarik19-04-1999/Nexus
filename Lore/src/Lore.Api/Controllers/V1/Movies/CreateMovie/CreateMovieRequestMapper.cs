using Lore.Application.Models.Enums;
using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.CreateMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class CreateMovieRequestMapper
{
    public static partial CreateMovieInput Map(CreateMovieRequest request);
}
