using Lore.Application.Models;

namespace Lore.Api.Controllers.V1.Universes.SearchUniverses;

public record SearchUniverseItem(int Id, string Name);

public static class SearchUniversesResponseMapper
{
    public static IReadOnlyList<SearchUniverseItem> Map(IReadOnlyList<SearchUniverseResult> results)
        => results.Select(x => new SearchUniverseItem(x.Id, x.Name)).ToList();
}
