using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class CreateUniverseUseCase : ICreateUniverseUseCase
{
    private readonly ILoreStore store;

    public CreateUniverseUseCase(ILoreStore store)
    {
        this.store = store;
    }

    public async Task<Result<Universe>> Execute(CreateUniverseInput input, CancellationToken cancellationToken = default)
    {
        var universe = new Universe
        {
            Name = input.Name,
            Description = input.Description,
            IsHidden = input.IsHidden,
            ListNo = input.ListNo,
        };

        await this.store.CreateUniverse(universe, cancellationToken);

        return Result<Universe>.Success(universe);
    }
}
