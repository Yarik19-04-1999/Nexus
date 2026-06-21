using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.GetMovieById;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetMovieByIdResponseMapper
{
    public static partial GetMovieByIdResponse Map(Movie movie);
}
