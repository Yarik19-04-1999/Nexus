using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class DeleteUniverseUseCase : IDeleteUniverseUseCase
{
    private readonly ILoreStore _store;

    public DeleteUniverseUseCase(ILoreStore store)
    {
        this._store = store;
    }

    public async Task<Result> Execute(DeleteUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = await this._store.GetUniverseById(input.Id, cancellationToken);
        if (universe is null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        await this._store.DeleteUniverse(universe, cancellationToken);

        return Result.Success();
    }
}
