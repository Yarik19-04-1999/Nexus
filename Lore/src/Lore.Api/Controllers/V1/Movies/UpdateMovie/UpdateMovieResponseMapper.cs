using Lore.Api.Controllers.V1.Movies.GetMovieById;
using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Movies.UpdateMovie;

public static class UpdateMovieResponseMapper
{
    public static GetMovieByIdResponse Map(Movie movie)
        => GetMovieByIdResponseMapper.Map(movie);
}
