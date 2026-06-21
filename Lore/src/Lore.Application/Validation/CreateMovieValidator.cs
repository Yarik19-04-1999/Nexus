using FluentValidation;
using Lore.Application.Constants;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class CreateMovieValidator : ValidatorBase<CreateMovieInput>, ICreateMovieValidator
{
    public CreateMovieValidator(CreateMovieValidationContext context)
    {
        RuleFor(x => x)
            .Must(_ => !context.MovieExists)
            .WithErrorCode(LoreErrorCodes.AlreadyExists)
            .WithMessage(x => LoreErrorMessages.MovieAlreadyExists(x.Title, x.ReleaseYear));
    }
}
