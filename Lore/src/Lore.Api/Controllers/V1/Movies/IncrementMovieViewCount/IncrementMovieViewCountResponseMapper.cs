using Lore.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.IncrementMovieViewCount;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class IncrementMovieViewCountResponseMapper
{
    public static partial IncrementMovieViewCountResponse Map(Movie movie);
}
