using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Interfaces;
using Nexus.Application.Core.Models;

namespace Lore.Application.Interfaces.UseCases;

public interface IUpdateUniverseUseCase : IUseCase<UpdateUniverseInput, Result<Universe>>
{
}
