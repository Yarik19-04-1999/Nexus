using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class UnlinkMovieFromUniverseUseCase : IUnlinkMovieFromUniverseUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public UnlinkMovieFromUniverseUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result> Execute(UnlinkMovieFromUniverseInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.MovieId, cancellationToken);
        var validationResult = await _validators.UnlinkMovieFromUniverseValidator(new MovieValidationContext(movie))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult();
        }

        movie!.UniverseId = null;
        movie.UpdatedAt = DateTime.UtcNow;
        await _store.UpdateMovie(movie, cancellationToken);
        return Result.Success();
    }
}
