using Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames.Dtos;

namespace Information.Api.Controllers.V1.EpicGames.GetEpicFreeGames;

public record GetEpicFreeGamesResponse(IReadOnlyList<EpicGameDto> Games);
