using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Lore.Application.Models.Mappers;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class UpdateUniverseUseCase : IUpdateUniverseUseCase
{
    private readonly ILoreStore _store;

    public UpdateUniverseUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<Universe>> Execute(UpdateUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = await _store.GetUniverseById(input.Id, cancellationToken);
        if (universe is null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        UpdateUniverseMapper.ApplyUpdate(input, universe);
        universe.UpdatedAt = DateTime.UtcNow;

        await _store.UpdateUniverse(universe, cancellationToken);

        return Result<Universe>.Success(universe);
    }
}
