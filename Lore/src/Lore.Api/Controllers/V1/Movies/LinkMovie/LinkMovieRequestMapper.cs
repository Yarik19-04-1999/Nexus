using Lore.Application.Models.Inputs;

namespace Lore.Api.Controllers.V1.Movies.LinkMovie;

public static class LinkMovieRequestMapper
{
    public static LinkMovieToUniverseInput Map(LinkMovieRequest request)
        => new(request.MovieId, request.UniverseId);
}
