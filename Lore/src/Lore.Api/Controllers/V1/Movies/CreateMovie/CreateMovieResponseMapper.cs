using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.CreateMovie;

public static class CreateMovieResponseMapper
{
    public static GetMovieByIdResponse Map(Movie movie)
        => GetMovieByIdResponseMapper.Map(movie);
}
