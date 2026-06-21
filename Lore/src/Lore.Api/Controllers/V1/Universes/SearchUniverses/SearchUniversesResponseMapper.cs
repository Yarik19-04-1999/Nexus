using Lore.Application.Models.Results;
using Riok.Mapperly.Abstractions;

namespace Lore.Api.Controllers.V1.Universes.SearchUniverses;

public record SearchUniverseItem(int Id, string Name);

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class SearchUniversesResponseMapper
{
    public static partial SearchUniverseItem Map(SearchUniverseResult result);

    public static IReadOnlyList<SearchUniverseItem> Map(IReadOnlyList<SearchUniverseResult> results)
        => results.Select(Map).ToList();
}
