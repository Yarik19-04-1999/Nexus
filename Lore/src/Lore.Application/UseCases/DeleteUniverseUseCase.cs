using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class DeleteUniverseUseCase : IDeleteUniverseUseCase
{
    private readonly ILoreStore store;

    public DeleteUniverseUseCase(ILoreStore store)
    {
        this.store = store;
    }

    public async Task<Result> Execute(DeleteUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = await this.store.GetUniverseById(input.Id, cancellationToken);
        if (universe is null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        await this.store.DeleteUniverse(universe, cancellationToken);

        return Result.Success();
    }
}
