using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class GetUniverseByIdUseCase : IGetUniverseByIdUseCase
{
    private readonly ILoreStore _store;

    public GetUniverseByIdUseCase(ILoreStore store)
    {
        this._store = store;
    }

    public async Task<Result<Universe>> Execute(GetUniverseByIdInput input, CancellationToken cancellationToken = default)
    {
        var universe = await this._store.GetUniverseById(input.Id, cancellationToken);
        if (universe is null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        return Result<Universe>.Success(universe);
    }
}
