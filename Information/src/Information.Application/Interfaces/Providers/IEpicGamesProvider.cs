using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IEpicGamesProvider
{
    Task<IReadOnlyList<EpicGiveaway>> GetCurrentGiveaways(CancellationToken cancellationToken = default);
}
