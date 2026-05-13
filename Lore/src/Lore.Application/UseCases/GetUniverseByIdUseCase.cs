using Lore.Application.Interfaces.UseCases;
using Lore.Application.Models;
using Lore.Application.Models.Inputs;
using Nexus.Application.Core.Models;

namespace Lore.Application.UseCases
{
    public class GetUniverseByIdUseCase : IGetUniverseByIdUseCase
    {
        public Task<Result<Universe>> ExecuteAsync(GetUniverseByIdInput input, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
