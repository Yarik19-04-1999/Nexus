using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class UpdateUniverseUseCase : IUpdateUniverseUseCase
{
    private readonly ILoreStore store;

    public UpdateUniverseUseCase(ILoreStore store)
    {
        this.store = store;
    }

    public async Task<Result<Universe>> Execute(UpdateUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = await this.store.GetUniverseById(input.Id, cancellationToken);
        if (universe is null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        universe.Name = input.Name;
        universe.Description = input.Description;
        universe.IsHidden = input.IsHidden;
        universe.ListNo = input.ListNo;
        universe.UpdatedAt = DateTime.UtcNow;

        await this.store.UpdateUniverse(universe, cancellationToken);

        return Result<Universe>.Success(universe);
    }
}
