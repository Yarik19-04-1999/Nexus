using FluentValidation;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class UnlinkMovieFromUniverseValidator : ValidatorBase<UnlinkMovieFromUniverseInput>, IUnlinkMovieFromUniverseValidator
{
    public UnlinkMovieFromUniverseValidator(MovieValidationContext context)
    {
        RuleFor(x => x)
            .Must(_ => context.Movie != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Movie>(x.MovieId));
    }
}
