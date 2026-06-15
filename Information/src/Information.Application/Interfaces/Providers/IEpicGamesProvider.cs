using Information.Application.Models;

namespace Information.Application.Interfaces.Providers;

public interface IEpicGamesProvider
{
    Task<IReadOnlyList<EpicGame>> GetFreeGames(CancellationToken cancellationToken = default);
}
