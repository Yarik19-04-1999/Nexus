using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.EpicGames.GetEpicGames;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetEpicGamesResponseMapper
{
    public static partial EpicGameDto Map(EpicGame game);
}
