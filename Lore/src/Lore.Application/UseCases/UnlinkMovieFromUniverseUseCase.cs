using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class UnlinkMovieFromUniverseUseCase : IUnlinkMovieFromUniverseUseCase
{
    private readonly ILoreStore _store;

    public UnlinkMovieFromUniverseUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result> Execute(UnlinkMovieFromUniverseInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.MovieId, cancellationToken);
        if (movie is null)
        {
            return LoreResultConstants.MovieNotFound(input.MovieId);
        }

        movie.UniverseId = null;
        movie.UpdatedAt = DateTime.UtcNow;

        await _store.UpdateMovie(movie, cancellationToken);
        return Result.Success();
    }
}
