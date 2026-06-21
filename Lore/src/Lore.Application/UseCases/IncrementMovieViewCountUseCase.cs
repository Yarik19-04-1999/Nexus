using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class IncrementMovieViewCountUseCase : IIncrementMovieViewCountUseCase
{
    private readonly ILoreStore _store;

    public IncrementMovieViewCountUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<Movie>> Execute(IncrementMovieViewCountInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        if (movie is null)
        {
            return LoreResultConstants.MovieNotFound(input.Id);
        }

        movie.ViewCount++;
        movie.UpdatedAt = DateTime.UtcNow;

        await _store.UpdateMovie(movie, cancellationToken);
        return Result<Movie>.Success(movie);
    }
}
