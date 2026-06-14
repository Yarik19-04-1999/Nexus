using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IEpicGamesProvider
{
    Task<Result<IReadOnlyList<EpicGame>>> GetFreeGames(CancellationToken cancellationToken = default);
}
