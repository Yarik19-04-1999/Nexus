using Information.Application.Interfaces.Providers;
using Information.Application.Interfaces.UseCases;
using Information.Application.Models;
using Information.Application.Models.Input;

namespace Information.Application.UseCases;

public class GetEpicFreeGamesUseCase : IGetEpicFreeGamesUseCase
{
    private readonly IEpicGamesProvider _epicGamesProvider;

    public GetEpicFreeGamesUseCase(IEpicGamesProvider epicGamesProvider)
    {
        _epicGamesProvider = epicGamesProvider;
    }

    public async Task<IReadOnlyList<EpicGame>> Execute(GetEpicFreeGamesInput input, CancellationToken cancellationToken = default)
        => await _epicGamesProvider.GetFreeGames(cancellationToken);
}
