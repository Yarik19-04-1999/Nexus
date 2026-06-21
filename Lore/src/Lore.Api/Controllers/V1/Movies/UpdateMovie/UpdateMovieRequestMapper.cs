using Lore.Application.Models.Enums;
using Lore.Application.Models.Inputs;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class UpdateMovieRequestMapper
{
    public static UpdateMovieInput Map(int id, UpdateMovieRequest request)
    {
        var input = MapFromRequest(request);
        return input with { Id = id };
    }

    [MapperIgnoreTarget(nameof(UpdateMovieInput.Id))]
    private static partial UpdateMovieInput MapFromRequest(UpdateMovieRequest request);
}
