using FluentValidation;
using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class UpdateMovieValidator : ValidatorBase<UpdateMovieInput>, IUpdateMovieValidator
{
    private readonly ILoreStore _store;

    public UpdateMovieValidator(ILoreStore store, UpdateMovieValidationContext context)
    {
        _store = store;

        RuleFor(x => x)
            .Must(_ => context.Movie != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Movie>(x.Id));

        RuleFor(x => x)
            .MustAsync(IsNotDuplicateTitleAndYear)
            .WithErrorCode(LoreErrorCodes.AlreadyExists)
            .WithMessage(x => LoreErrorMessages.MovieAlreadyExists(x.Title, x.ReleaseYear));

        RuleFor(x => x.UniverseId)
            .MustAsync(IsUniverseExists)
            .When(x => x.UniverseId.HasValue)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Universe>(x.UniverseId!.Value));
    }

    private async Task<bool> IsNotDuplicateTitleAndYear(UpdateMovieInput input, CancellationToken ct)
        => !await _store.OtherMovieExistsByTitleAndYear(input.Title, input.ReleaseYear, input.Id, ct);

    private Task<bool> IsUniverseExists(int? universeId, CancellationToken ct)
        => _store.UniverseExistsById(universeId!.Value, ct);
}
