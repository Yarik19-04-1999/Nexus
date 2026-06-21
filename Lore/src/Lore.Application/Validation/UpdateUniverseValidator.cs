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

public class UpdateUniverseValidator : ValidatorBase<UpdateUniverseInput>, IUpdateUniverseValidator
{
    private readonly ILoreStore _store;

    public UpdateUniverseValidator(ILoreStore store, UpdateUniverseValidationContext context)
    {
        _store = store;

        RuleFor(x => x)
            .Must(_ => context.Universe != null)
            .WithErrorCode(CommonErrorCodes.NotFound)
            .WithMessage(x => CommonErrorMessages.NotFound<Universe>(x.Id));

        RuleFor(x => x.Name)
            .MustAsync(IsNotDuplicateName)
            .WithErrorCode(LoreErrorCodes.AlreadyExists)
            .WithMessage(x => LoreErrorMessages.UniverseAlreadyExists(x.Name));
    }

    private async Task<bool> IsNotDuplicateName(UpdateUniverseInput input, string name, CancellationToken ct)
        => !await _store.OtherUniverseExistsByName(name, input.Id, ct);
}
