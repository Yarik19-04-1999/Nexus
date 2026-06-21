using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class GetUniversesUseCase : IGetUniversesUseCase
{
    private readonly ILoreStore _store;

    public GetUniversesUseCase(ILoreStore store)
    {
        this._store = store;
    }

    public async Task<Result<PagedResult<Universe>>> Execute(GetUniversesInput input, CancellationToken cancellationToken = default)
    {
        var result = await this._store.GetUniversesPaged(input.Sieve, cancellationToken);
        return Result<PagedResult<Universe>>.Success(result);
    }
}
