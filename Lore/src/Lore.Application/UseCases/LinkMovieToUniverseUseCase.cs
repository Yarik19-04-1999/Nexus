using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class LinkMovieToUniverseUseCase : ILinkMovieToUniverseUseCase
{
    private readonly ILoreStore _store;

    public LinkMovieToUniverseUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result> Execute(LinkMovieToUniverseInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.MovieId, cancellationToken);
        if (movie is null)
        {
            return LoreResultConstants.MovieNotFound(input.MovieId);
        }

        var universe = await _store.GetUniverseById(input.UniverseId, cancellationToken);
        if (universe is null)
        {
            return LoreResultConstants.UniverseNotFoundForMovie(input.UniverseId);
        }

        movie.UniverseId = input.UniverseId;
        movie.UpdatedAt = DateTime.UtcNow;

        await _store.UpdateMovie(movie, cancellationToken);
        return Result.Success();
    }
}
