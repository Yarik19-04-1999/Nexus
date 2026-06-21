using FluentValidation;
using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class CreateMovieValidator : ValidatorBase<CreateMovieInput>, ICreateMovieValidator
{
    public CreateMovieValidator(ILoreStore store)
    {
        RuleFor(x => x)
            .MustAsync(async (input, ct) =>
                !await store.MovieExistsByTitleAndYear(input.Title, input.ReleaseYear, ct))
            .WithErrorCode(LoreErrorCodes.AlreadyExists)
            .WithMessage(x => LoreErrorMessages.MovieAlreadyExists(x.Title, x.ReleaseYear));
    }
}
