using FluentValidation;
using Lore.Application.Constants;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class DecrementMovieViewCountValidator : ValidatorBase<DecrementMovieViewCountInput>, IDecrementMovieViewCountValidator
{
    public DecrementMovieViewCountValidator(DecrementMovieViewCountValidationContext context)
    {
        RuleFor(x => x)
            .Must(_ => context.Movie != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Movie>(x.Id));

        RuleFor(x => x)
            .Must(_ => context.Movie!.ViewCount > 0)
            .WithErrorCode(LoreErrorCodes.ViewCountAlreadyZero)
            .WithMessage(x => LoreErrorMessages.ViewCountAlreadyZero(x.Id));
    }
}
