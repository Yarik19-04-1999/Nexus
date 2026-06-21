using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class DeleteMovieUseCase : IDeleteMovieUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public DeleteMovieUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result> Execute(DeleteMovieInput input, CancellationToken cancellationToken = default)
    {
        var movie = await _store.GetMovieById(input.Id, cancellationToken);
        var validationResult = await _validators.CreateDeleteMovieValidator(new DeleteMovieValidationContext(movie))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult();
        }

        await _store.DeleteMovie(movie!, cancellationToken);
        return Result.Success();
    }
}
