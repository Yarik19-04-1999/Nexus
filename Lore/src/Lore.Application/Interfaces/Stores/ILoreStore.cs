using Lore.Application.Models;

namespace Lore.Application.Interfaces.Stores;

public interface ILoreStore
{
    public Task<Universe?> GetUniverseById(int id, CancellationToken cancellationToken);
}
