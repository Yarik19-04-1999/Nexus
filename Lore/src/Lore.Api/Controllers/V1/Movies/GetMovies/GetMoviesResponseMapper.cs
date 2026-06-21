using Lore.Application.Models;
using Lore.Application.Models.Enums;
using Nexus.Application.Core.Models;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Movies.GetMovies;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetMoviesResponseMapper
{
    public static GetMoviesResponse Map(PagedResult<Movie> result)
        => new(result.Items.Select(Map).ToList(), result.TotalCount, result.Page, result.PageSize, result.TotalPages);

    public static partial GetMovieItemResponse Map(Movie movie);
}
