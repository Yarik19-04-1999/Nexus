using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class UpdateMovieUseCase : IUpdateMovieUseCase
{
    private readonly ILoreStore _store;

    public UpdateMovieUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<Movie>> Execute(UpdateMovieInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        if (movie is null)
        {
            return LoreResultConstants.MovieNotFound(input.Id);
        }

        UpdateMovieMapper.ApplyUpdate(input, movie);
        movie.UpdatedAt = DateTime.UtcNow;

        await _store.UpdateMovie(movie, cancellationToken);
        return Result<Movie>.Success(movie);
    }
}
