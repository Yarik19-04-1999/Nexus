using Information.Application.Interfaces.Providers;
using Information.Application.Models;

namespace Information.Infrastructure.Providers;

public class EpicGamesProvider : IEpicGamesProvider
{
    public Task<IReadOnlyList<EpicGiveaway>> GetCurrentGiveaways(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
