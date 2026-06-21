using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Interfaces.Validators;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Nexus.Application.Core.Models;
using Nexus.Application.Core.Validation;

namespace Lore.Application.UseCases;

public class CreateUniverseUseCase : ICreateUniverseUseCase
{
    private readonly ILoreStore _store;
    private readonly ILoreValidatorFactory _validators;

    public CreateUniverseUseCase(ILoreStore store, ILoreValidatorFactory validators)
    {
        _store = store;
        _validators = validators;
    }

    public async Task<Result<Universe>> Execute(CreateUniverseInput input, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validators.CreateCreateUniverseValidator()
            .ValidateAsync(input, cancellationToken);

        if (!validationResult.IsValid)
        {
            return validationResult.ToResult<Universe>();
        }

        var universe = CreateUniverseMapper.Map(input);
        await _store.CreateUniverse(universe, cancellationToken);
        return Result<Universe>.Success(universe);
    }
}
