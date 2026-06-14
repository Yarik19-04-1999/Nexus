using Information.Application.Constants;
using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;
using Nexus.Application.Core.Models;

namespace Information.Application.UseCases;

public class GetEpicFreeGamesUseCase : IGetEpicFreeGamesUseCase
{
    private readonly IEpicGamesProvider _epicGamesProvider;

    public GetEpicFreeGamesUseCase(IEpicGamesProvider epicGamesProvider)
    {
        _epicGamesProvider = epicGamesProvider;
    }

    public async Task<Result<IReadOnlyList<EpicGame>>> Execute(GetEpicFreeGamesInput input, CancellationToken cancellationToken = default)
    {
        var result = await _epicGamesProvider.GetFreeGames(cancellationToken);

        if (result.HasError)
        {
            return InformationResultConstants.ProviderUnavailable<IReadOnlyList<EpicGame>>(nameof(IEpicGamesProvider));
        }

        return result;
    }
}
