using FluentValidation;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class LinkMovieToUniverseValidator : ValidatorBase<LinkMovieToUniverseInput>, ILinkMovieToUniverseValidator
{
    public LinkMovieToUniverseValidator(ILoreStore store, LinkMovieToUniverseValidationContext context)
    {
        RuleFor(x => x)
            .Must(_ => context.Movie != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Movie>(x.MovieId));

        RuleFor(x => x.UniverseId)
            .MustAsync((id, ct) => store.UniverseExistsById(id, ct))
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Universe>(x.UniverseId));
    }
}
