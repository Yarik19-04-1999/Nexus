using FluentValidation;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class DeleteUniverseValidator : ValidatorBase<DeleteUniverseInput>, IDeleteUniverseValidator
{
    public DeleteUniverseValidator(UniverseValidationContext context)
    {
        RuleFor(x => x)
            .Must(_ => context.Universe != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Universe>(x.Id));
    }
}
