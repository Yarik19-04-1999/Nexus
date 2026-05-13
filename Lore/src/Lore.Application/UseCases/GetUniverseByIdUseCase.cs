using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Constants;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class GetUniverseByIdUseCase : IGetUniverseByIdUseCase
{
    private readonly ILoreStore store;

    public GetUniverseByIdUseCase(
        ILoreStore store) 
    {
        this.store = store;
    }

    public async Task<Result<Universe>> ExecuteAsync(GetUniverseByIdInput input, CancellationToken cancellationToken = default)
    {
        var universe = await this.store.GetUniverseById(input.Id, cancellationToken);
        if (universe == null)
        {
            return ResultConstants.NotFound<Universe>(input.Id);
        }

        return Result<Universe>.Success(universe);
    }
}
