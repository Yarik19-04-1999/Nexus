using FluentValidation;
using Lore.Application.Constants;
using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Validation;

namespace Lore.Application.Validation;

public class CreateUniverseValidator : ValidatorBase<CreateUniverseInput>, ICreateUniverseValidator
{
    public CreateUniverseValidator(ILoreStore store)
    {
        RuleFor(x => x.Name)
            .MustAsync(async (name, ct) => !await store.UniverseExistsByName(name, ct))
            .WithErrorCode(LoreErrorCodes.AlreadyExists)
            .WithMessage(x => LoreErrorMessages.UniverseAlreadyExists(x.Name));
    }
}
