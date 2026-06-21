using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class CreateUniverseUseCase : ICreateUniverseUseCase
{
    private readonly ILoreStore _store;

    public CreateUniverseUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<Universe>> Execute(CreateUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = CreateUniverseMapper.MapCreate(input);
        await _store.CreateUniverse(universe, cancellationToken);
        return Result<Universe>.Success(universe);
    }
}
