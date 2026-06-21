using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class UpdateMovieUseCase : IUpdateMovieUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public UpdateMovieUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result<Movie>> Execute(UpdateMovieInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        var validationResult = await _validators.CreateUpdateMovieValidator(new UpdateMovieValidationContext(movie))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Movie>();
        }

        UpdateMovieMapper.Map(input, movie!);
        movie!.UpdatedAt = DateTime.UtcNow;
        await _store.UpdateMovie(movie, cancellationToken);
        return Result<Movie>.Success(movie);
    }
}
