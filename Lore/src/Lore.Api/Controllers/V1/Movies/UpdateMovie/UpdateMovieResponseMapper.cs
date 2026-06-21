using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class UpdateMovieResponseMapper
{
    public static partial GetMovieByIdResponse Map(Movie movie);
}
