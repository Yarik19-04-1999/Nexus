using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class GetUniversesUseCase : IGetUniversesUseCase
{
    private readonly ILoreStore store;

    public GetUniversesUseCase(ILoreStore store)
    {
        this.store = store;
    }

    public async Task<Result<PagedResult<Universe>>> Execute(GetUniversesInput input, CancellationToken cancellationToken = default)
    {
        var result = await this.store.GetUniversesPaged(input.Sieve, cancellationToken);
        return Result<PagedResult<Universe>>.Success(result);
    }
}
