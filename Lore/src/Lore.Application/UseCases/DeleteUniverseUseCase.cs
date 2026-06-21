using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class DeleteUniverseUseCase : IDeleteUniverseUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public DeleteUniverseUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result> Execute(DeleteUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = await _store.GetUniverseById(input.Id, cancellationToken);
        var validationResult = await _validators.CreateDeleteUniverseValidator(new DeleteUniverseValidationContext(universe))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult();
        }

        await _store.DeleteUniverse(universe!, cancellationToken);
        return Result.Success();
    }
}
