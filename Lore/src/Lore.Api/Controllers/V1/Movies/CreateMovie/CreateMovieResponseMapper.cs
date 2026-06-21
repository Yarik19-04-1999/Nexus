using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.CreateMovie;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class CreateMovieResponseMapper
{
    public static partial GetMovieByIdResponse Map(Movie movie);
}
