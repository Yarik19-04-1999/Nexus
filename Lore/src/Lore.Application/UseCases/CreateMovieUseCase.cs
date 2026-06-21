using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class CreateMovieUseCase : ICreateMovieUseCase
{
    private readonly ILoreStore _store;

    public CreateMovieUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<Movie>> Execute(CreateMovieInput input, CancellationToken cancellationToken = default)
    {
        var movie = CreateMovieMapper.Map(input);
        await _store.CreateMovie(movie, cancellationToken);
        return Result<Movie>.Success(movie);
    }
}
