using Lore.Application.Interfaces.Stores;
using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models.Results;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases;

public class SearchUniversesUseCase : ISearchUniversesUseCase
{
    private readonly ILoreStore _store;

    public SearchUniversesUseCase(ILoreStore store)
    {
        _store = store;
    }

    public async Task<Result<IReadOnlyList<SearchUniverseResult>>> Execute(SearchUniversesInput input, CancellationToken cancellationToken = default)
    {
        var universes = await _store.SearchUniverses(input.Query, cancellationToken);
        var results = universes.Select(x => new SearchUniverseResult(x.Id, x.Name)).ToList();
        return Result<IReadOnlyList<SearchUniverseResult>>.Success(results);
    }
}
