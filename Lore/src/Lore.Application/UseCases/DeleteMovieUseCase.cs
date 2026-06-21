using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly ILoreStore _store;

    public DeleteMovieUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result> Execute(DeleteMovieInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        if (movie is null)
        {
            return LoreResultConstants.MovieNotFound(input.Id);
        }

        await _store.DeleteMovie(movie, cancellationToken);
        return Result.Success();
    }
}
