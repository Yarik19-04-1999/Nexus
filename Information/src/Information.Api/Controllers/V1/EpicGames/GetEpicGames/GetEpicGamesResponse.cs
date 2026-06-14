namespace Information.Api.Controllers.V1.EpicGames.GetEpicGames;

public record GetEpicGamesResponse(IReadOnlyList<EpicGameDto> Games);
