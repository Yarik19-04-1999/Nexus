using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Movies.UnlinkMovie;

public static class UnlinkMovieRequestMapper
{
    public static UnlinkMovieFromUniverseInput Map(UnlinkMovieRequest request)
        => new(request.MovieId);
}
