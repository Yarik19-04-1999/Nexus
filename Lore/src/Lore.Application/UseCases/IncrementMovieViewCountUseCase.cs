using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class IncrementMovieViewCountUseCase : IIncrementMovieViewCountUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public IncrementMovieViewCountUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result<Movie>> Execute(IncrementMovieViewCountInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        var validationResult = await _validators.CreateIncrementMovieViewCountValidator(new IncrementMovieViewCountValidationContext(movie))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Movie>();
        }

        movie!.ViewCount++;
        movie.UpdatedAt = DateTime.UtcNow;
        await _store.UpdateMovie(movie, cancellationToken);
        return Result<Movie>.Success(movie);
    }
}
