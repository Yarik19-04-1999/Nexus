using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.ValidationContexts;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class GetUniverseByIdUseCase : IGetUniverseByIdUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public GetUniverseByIdUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result<Universe>> Execute(GetUniverseByIdInput input, CancellationToken cancellationToken = default)
    {
        var universe = await _store.GetUniverseById(input.Id, cancellationToken);
        var validationResult = await _validators.GetUniverseByIdValidator(new UniverseValidationContext(universe))
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Universe>();
        }

        return Result<Universe>.Success(universe!);
    }
}
