using Lore.Application.Models.Results;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Lore.Application.Interfaces.UseCases;

public interface ISearchUniversesUseCase : IUseCase<SearchUniversesInput, Result<IReadOnlyList<SearchUniverseResult>>>
{
}
