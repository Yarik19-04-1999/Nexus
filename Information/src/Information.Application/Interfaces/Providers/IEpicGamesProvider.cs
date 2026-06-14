using Information.Application.Models;
using Nexus.Application.Core.Models;

namespace Information.Application.Interfaces.Providers;

public interface IEpicGamesProvider
{
    Task<Result<IReadOnlyList<EpicGame>>> GetEpicGames(CancellationToken cancellationToken = default);
}
