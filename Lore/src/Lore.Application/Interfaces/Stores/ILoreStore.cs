using Lore.Application.Models;
using Nexus.Application.Core.Models;
using Sieve.Models;

namespace Lore.Application.Interfaces.Stores;

public interface ILoreStore
{
    Task<PagedResult<Universe>> GetUniversesPaged(SieveModel sieve, CancellationToken cancellationToken);
    Task<Universe?> GetUniverseById(int id, CancellationToken cancellationToken);
    Task CreateUniverse(Universe universe, CancellationToken cancellationToken);
    Task UpdateUniverse(Universe universe, CancellationToken cancellationToken);
    Task DeleteUniverse(Universe universe, CancellationToken cancellationToken);
}
