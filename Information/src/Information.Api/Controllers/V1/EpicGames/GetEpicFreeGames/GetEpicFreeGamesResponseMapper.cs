using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames.Dtos;
using Information.Application.Models;
using Riok.Mapperly.Abstractions;

namespace Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class GetEpicFreeGamesResponseMapper
{
    public static partial EpicGameDto Map(EpicGame game);
}
