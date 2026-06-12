using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetEpicGiveawaysUseCase : IGetEpicGiveawaysUseCase
{
    private readonly IEpicGamesProvider _provider;

    public GetEpicGiveawaysUseCase(IEpicGamesProvider provider)
    {
        _provider = provider;
    }

    public async Task<Result<IReadOnlyList<EpicGiveaway>>> Execute(GetEpicGiveawaysInput input, CancellationToken cancellationToken = default)
    {
        var giveaways = await _provider.GetCurrentGiveaways(cancellationToken);
        return Result<IReadOnlyList<EpicGiveaway>>.Success(giveaways);
    }
}
