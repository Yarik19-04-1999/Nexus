using Lore.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.DecrementMovieViewCount;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class DecrementMovieViewCountResponseMapper
{
    public static partial DecrementMovieViewCountResponse Map(Movie movie);
}
